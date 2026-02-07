using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class AdminWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна администратора
        public AdminWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок с ФИО администратора
            this.Title = $"Администратор: {_currentUser.FirstName} {_currentUser.LastName}";

            //загружаем все данные
            LoadData();
        }

        //перевод: загрузка всех данных
        private void LoadData()
        {
            //загружаем пользователей и сотрудников
            LoadUsers();
            LoadEmployees();
        }

        //перевод: загрузка пользователей
        private void LoadUsers()
        {
            //пробуем загрузить пользователей с ролями из БД
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var users = dbContext.Users.Include(u => u.Role).ToList();
                    dgUsers.ItemsSource = users;
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку если не удалось загрузить
                ShowErrorMessage("Ошибка загрузки пользователей", ex.Message);
            }
        }

        //перевод: добавление нового пользователя
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            ShowUserEditWindow();
        }

        //перевод: редактирование выбранного пользователя
        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedUser();
        }

        //перевод: удаление выбранного пользователя
        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedUser();
        }

        //перевод: отображение окна редактирования пользователя
        private void ShowUserEditWindow(User user = null)
        {
            //создаём окно для нового или существующего пользователя
            var editWindow = user == null ? new UserEditWindow() : new UserEditWindow(user);

            //показываем диалог и перезагружаем если сохранено
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                LoadUsers();
            }
        }

        //перевод: редактирование выбранного пользователя
        private void EditSelectedUser()
        {
            //проверяем выбран ли пользователь
            if (dgUsers.SelectedItem == null)
            {
                ShowWarningMessage("Выберите пользователя для редактирования");
                return;
            }

            //получаем выбранного и свежего из БД
            var selectedUser = dgUsers.SelectedItem as User;
            var userFromDb = GetUserFromDatabase(selectedUser.UserId);

            //открываем редактирование если найден
            if (userFromDb != null)
            {
                ShowUserEditWindow(userFromDb);
            }
        }

        //перевод: получение пользователя из базы данных
        private User GetUserFromDatabase(int userId)
        {
            //пробуем получить пользователя с ролью
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    return dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку если не удалось
                ShowErrorMessage("Ошибка получения пользователя", ex.Message);
                return null;
            }
        }

        //перевод: удаление выбранного пользователя
        private void DeleteSelectedUser()
        {
            //проверяем выбран ли пользователь
            if (dgUsers.SelectedItem == null)
            {
                ShowWarningMessage("Выберите пользователя для удаления");
                return;
            }

            var selectedUser = dgUsers.SelectedItem as User;

            //подтверждаем и удаляем если да
            if (ConfirmUserDeletion(selectedUser))
            {
                DeleteUserFromDatabase(selectedUser.UserId);
            }
        }

        //перевод: подтверждение удаления пользователя
        private bool ConfirmUserDeletion(User user)
        {
            //формируем сообщение с ФИО и логином
            var message = $"Вы уверены, что хотите удалить пользователя:\n" +
                          $"{user.LastName} {user.FirstName}?\n" +
                          $"Логин: {user.Login}";

            //возвращаем true если да
            return MessageBox.Show(message, "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: удаление пользователя из базы данных
        private void DeleteUserFromDatabase(int userId)
        {
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    // Находим пользователя
                    var userToDelete = dbContext.Users
                        .FirstOrDefault(u => u.UserId == userId);

                    if (userToDelete == null)
                    {
                        ShowWarningMessage("Пользователь не найден");
                        return;
                    }

                    // Удаляем связанные записи (в порядке, обратном зависимостям)

                    // 1. invoices (если пользователь — клиент)
                    var invoices = dbContext.Invoices
                        .Where(i => i.ClientUserId == userId)
                        .ToList();
                    dbContext.Invoices.RemoveRange(invoices);

                    // 2. trips (если пользователь — водитель)
                    var trips = dbContext.Trips
                        .Where(t => t.DriverUserId == userId)
                        .ToList();
                    dbContext.Trips.RemoveRange(trips);

                    // 3. applications (заявки, созданные пользователем)
                    var applications = dbContext.Applications
                        .Where(a => a.UserId == userId)
                        .ToList();
                    dbContext.Applications.RemoveRange(applications);

                    // 4. client_profiles (профиль клиента)
                    var clientProfile = dbContext.ClientProfiles
                        .FirstOrDefault(cp => cp.UserId == userId);
                    if (clientProfile != null)
                    {
                        dbContext.ClientProfiles.Remove(clientProfile);
                    }

                    // 5. employees (сотрудник)
                    var employee = dbContext.Employees
                        .FirstOrDefault(e => e.UserId == userId);
                    if (employee != null)
                    {
                        // Если у сотрудника есть зависимости (например batches, trips по employee_id)
                        var batches = dbContext.Batches
                            .Where(b => b.AcceptedByEmployeeId == employee.EmployeeId)
                            .ToList();
                        dbContext.Batches.RemoveRange(batches);

                        // trips по employee_id (driver_employee_id, dispatcher_employee_id и т.д.)
                        var employeeTrips = dbContext.Trips
                            .Where(t => t.DriverEmployeeId == employee.EmployeeId ||
                                       t.DispatcherEmployeeId == employee.EmployeeId /* и другие поля */)
                            .ToList();
                        dbContext.Trips.RemoveRange(employeeTrips);

                        dbContext.Employees.Remove(employee);
                    }

                    // Теперь можно безопасно удалить пользователя
                    dbContext.Users.Remove(userToDelete);

                    dbContext.SaveChanges();

                    ShowSuccessMessage("Пользователь и связанные данные успешно удалены");
                    LoadUsers();
                }
            }
            catch (DbUpdateException ex)
            {
                string innerMsg = ex.InnerException?.Message ?? ex.Message;
                ShowErrorMessage("Не удалось удалить пользователя", innerMsg);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Ошибка при удалении", ex.Message);
            }
        }

        //перевод: загрузка сотрудников
        private void LoadEmployees()
        {
            //пробуем загрузить сотрудников с позициями и пользователями
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var employees = dbContext.Employees
                        .Include(e => e.Position)
                        .Include(e => e.User)
                        .ToList();
                    dgEmployees.ItemsSource = employees;
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку
                ShowErrorMessage("Ошибка загрузки сотрудников", ex.Message);
            }
        }

        //перевод: добавление нового сотрудника
        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            ShowEmployeeEditWindow();
        }

        //перевод: редактирование выбранного сотрудника
        private void BtnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedEmployee();
        }

        //перевод: удаление выбранного сотрудника
        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedEmployee();
        }

        //перевод: отображение окна редактирования сотрудника
        private void ShowEmployeeEditWindow(Employee employee = null)
        {
            //создаём окно для нового или существующего сотрудника
            var editWindow = employee == null ? new EmployeeEditWindow() : new EmployeeEditWindow(employee);

            //показываем диалог и перезагружаем если сохранено
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                LoadEmployees();
            }
        }

        //перевод: редактирование выбранного сотрудника
        private void EditSelectedEmployee()
        {
            //проверяем выбран ли сотрудник
            if (dgEmployees.SelectedItem == null)
            {
                ShowWarningMessage("Выберите сотрудника для редактирования");
                return;
            }

            var selectedEmployee = dgEmployees.SelectedItem as Employee;
            var employeeFromDb = GetEmployeeFromDatabase(selectedEmployee.EmployeeId);

            //открываем редактирование если найден
            if (employeeFromDb != null)
            {
                ShowEmployeeEditWindow(employeeFromDb);
            }
        }

        //перевод: получение сотрудника из базы данных
        private Employee GetEmployeeFromDatabase(int employeeId)
        {
            //пробуем получить сотрудника с позицией и пользователем
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    return dbContext.Employees
                        .Include(e => e.Position)
                        .Include(e => e.User)
                        .FirstOrDefault(e => e.EmployeeId == employeeId);
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку
                ShowErrorMessage("Ошибка получения сотрудника", ex.Message);
                return null;
            }
        }

        //перевод: удаление выбранного сотрудника
        private void DeleteSelectedEmployee()
        {
            //проверяем выбран ли сотрудник
            if (dgEmployees.SelectedItem == null)
            {
                ShowWarningMessage("Выберите сотрудника для удаления");
                return;
            }

            var selectedEmployee = dgEmployees.SelectedItem as Employee;

            //подтверждаем и удаляем если да
            if (ConfirmEmployeeDeletion(selectedEmployee))
            {
                DeleteEmployeeFromDatabase(selectedEmployee.EmployeeId);
            }
        }

        //перевод: подтверждение удаления сотрудника
        private bool ConfirmEmployeeDeletion(Employee employee)
        {
            //формируем сообщение с ФИО и табельным номером
            var message = $"Вы уверены, что хотите удалить сотрудника:\n" +
                          $"{employee.User?.LastName} {employee.User?.FirstName}?\n" +
                          $"Табельный номер: {employee.EmployeeNumber}";

            //возвращаем true если да
            return MessageBox.Show(message, "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: удаление сотрудника из базы данных
        private void DeleteEmployeeFromDatabase(int employeeId)
        {
            //пробуем удалить
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var employeeToDelete = dbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                    if (employeeToDelete == null) return;

                    dbContext.Employees.Remove(employeeToDelete);
                    dbContext.SaveChanges();

                    //показываем успех и перезагружаем
                    ShowSuccessMessage("Сотрудник успешно удален");
                    LoadEmployees();
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку
                ShowErrorMessage("Ошибка при удалении", ex.Message);
            }
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя из системы
        private void LogoutUser()
        {
            //подтверждаем и переходим если да
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение выхода
        private bool ConfirmLogout()
        {
            //возвращаем true если да
            return MessageBox.Show("Выйти из системы?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: возврат к окну входа
        private void ReturnToLoginWindow()
        {
            //открываем логин и закрываем текущее
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        //перевод: отображение сообщения об ошибке
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show($"Ошибка: {message}", title,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //перевод: отображение предупреждающего сообщения
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Внимание",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //перевод: отображение сообщения об успехе
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}