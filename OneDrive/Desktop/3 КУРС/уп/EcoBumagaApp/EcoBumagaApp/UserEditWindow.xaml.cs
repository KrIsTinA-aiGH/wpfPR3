using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EcoBumagaApp
{
    public partial class UserEditWindow : Window
    {
        private readonly User? _userToEdit;
        private readonly bool _isEditMode;

        // Конструктор для создания нового пользователя
        public UserEditWindow()
        {
            InitializeComponent();
            _isEditMode = false;
            Title = "Добавление пользователя";
            LoadRoles();
        }

        // Конструктор для редактирования существующего пользователя
        public UserEditWindow(User user) : this()
        {
            _userToEdit = user;
            _isEditMode = true;
            Title = "Редактирование пользователя";
            LoadUserData();
        }

        // Загружает список ролей в ComboBox
        // Загрузка ролей
        private void LoadRoles()
        {
            try
            {
                using var db = new EcoBumagaDbContext();
                var roles = db.Roles.ToList();
                cbRole.ItemsSource = roles;

                if (roles.Count > 0)
                    cbRole.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                ShowErrorMessage("Не удалось загрузить роли", ex.Message);
            }
        }

        // Загружает данные редактируемого пользователя в форму
        // Загрузка данных пользователя
        private void LoadUserData()
        {
            if (_userToEdit == null) return;

            try
            {
                using var db = new EcoBumagaDbContext();
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.UserId == _userToEdit.UserId);

                if (user == null)
                {
                    ShowErrorMessage("Пользователь не найден", "Запись удалена или недоступна");
                    return;
                }

                txtLastName.Text = user.LastName;
                txtFirstName.Text = user.FirstName;
                txtPatronymic.Text = user.Patronymic;
                txtLogin.Text = user.Login;
                SelectRole(user.RoleId);
            }
            catch (System.Exception ex)
            {
                ShowErrorMessage("Ошибка загрузки данных", ex.Message);
            }
        }

        // Устанавливает выбранную роль в ComboBox по ID
        // Выбор роли в выпадающем списке
        private void SelectRole(int roleId)
        {
            foreach (var item in cbRole.Items)
            {
                if (item is Role role && role.RoleId == roleId)
                {
                    cbRole.SelectedItem = role;
                    return;
                }
            }
        }

        // Обработчик кнопки "Сохранить"
        // Сохранение пользователя
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;
            SaveOrUpdateUser();
        }

        // Проверяет корректность заполнения всех обязательных полей
        // Валидация формы
        private bool ValidateForm()
        {
            ClearError();

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                return ShowFieldError(txtFirstName, "Имя обязательно");

            if (string.IsNullOrWhiteSpace(txtLogin.Text))
                return ShowFieldError(txtLogin, "Логин обязателен");

            if (txtLogin.Text.Length < 3)
                return ShowFieldError(txtLogin, "Логин слишком короткий (мин. 3 символа)");

            if (!_isEditMode && string.IsNullOrWhiteSpace(txtPassword.Password))
                return ShowFieldError(txtPassword, "Пароль обязателен при создании");

            if (!string.IsNullOrWhiteSpace(txtPassword.Password) && txtPassword.Password.Length < 4)
                return ShowFieldError(txtPassword, "Пароль слишком короткий (мин. 4 символа)");

            if (!string.IsNullOrWhiteSpace(txtPassword.Password) &&
                txtPassword.Password != txtPasswordConfirm.Password)
                return ShowFieldError(txtPasswordConfirm, "Пароли не совпадают");

            if (cbRole.SelectedItem == null)
            {
                ShowError("Выберите роль");
                return false;
            }

            return true;
        }

        // Показывает ошибку и фокусируется на поле
        private bool ShowFieldError(Control control, string message)
        {
            ShowError(message);
            control.Focus();
            return false;
        }

        // Создаёт или обновляет пользователя в базе данных
        // Сохранение / обновление пользователя
        private void SaveOrUpdateUser()
        {
            try
            {
                using var db = new EcoBumagaDbContext();
                var selectedRole = (Role)cbRole.SelectedItem!;

                if (_isEditMode)
                {
                    var user = db.Users.FirstOrDefault(u => u.UserId == _userToEdit!.UserId);
                    if (user == null)
                    {
                        ShowError("Пользователь не найден в базе");
                        return;
                    }

                    UpdateUserProperties(user, selectedRole);
                }
                else
                {
                    var newUser = new User
                    {
                        LastName = txtLastName.Text?.Trim(),
                        FirstName = txtFirstName.Text.Trim(),
                        Patronymic = txtPatronymic.Text?.Trim(),
                        Login = txtLogin.Text.Trim(),
                        PasswordHash = txtPassword.Password,
                        RoleId = selectedRole.RoleId
                    };
                    db.Users.Add(newUser);
                }

                db.SaveChanges();
                ShowSuccessAndClose();
            }
            catch (DbUpdateException)
            {
                ShowFieldError(txtLogin, $"Логин '{txtLogin.Text}' уже занят");
            }
            catch (System.Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Обновляет свойства существующего пользователя
        // Обновление данных пользователя
        private void UpdateUserProperties(User user, Role role)
        {
            user.LastName = txtLastName.Text?.Trim();
            user.FirstName = txtFirstName.Text.Trim();
            user.Patronymic = txtPatronymic.Text?.Trim();
            user.Login = txtLogin.Text.Trim();
            user.RoleId = role.RoleId;

            if (!string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                user.PasswordHash = txtPassword.Password;
            }
        }

        // Показывает сообщение об успехе и закрывает окно
        // Успешное завершение
        private void ShowSuccessAndClose()
        {
            MessageBox.Show(
                _isEditMode ? "Пользователь успешно обновлён" : "Пользователь успешно добавлен",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        // Обработчик кнопки "Отмена"
        // Отмена и закрытие окна
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Показывает текстовую ошибку под формой
        // Показ ошибки
        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }

        // Очищает сообщение об ошибке
        // Очистка ошибки
        private void ClearError()
        {
            txtError.Text = "";
            txtError.Visibility = Visibility.Collapsed;
        }

        // Показывает всплывающее сообщение об ошибке
        // Показ системной ошибки
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show($"Ошибка: {message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}