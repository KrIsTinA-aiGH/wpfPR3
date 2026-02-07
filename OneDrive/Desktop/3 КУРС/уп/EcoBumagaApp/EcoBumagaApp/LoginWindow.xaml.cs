using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class LoginWindow : Window
    {
        //перевод: конструктор окна входа
        public LoginWindow()
        {
            //инициализируем компоненты интерфейса
            InitializeComponent();

            //устанавливаем фокус на поле логина для удобства
            txtLogin.Focus();
        }

        //перевод: обработчик нажатия кнопки входа
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //запускаем процесс аутентификации
            AuthenticateUser();
        }

        //перевод: обработчик нажатия кнопки гостевого входа
        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            //открываем гостевое окно
            OpenGuestWindow();
        }

        //перевод: аутентификация пользователя
        private void AuthenticateUser()
        {
            //проверяем введённые данные, если ошибка — выходим
            if (!ValidateCredentials()) return;

            //пробуем получить пользователя и проверить
            try
            {
                var user = GetUserFromDatabase();
                if (user == null)
                {
                    ShowError("Пользователь не найден");
                    return;
                }

                if (!VerifyPassword(user))
                {
                    ShowError("Неверный пароль");
                    return;
                }

                //открываем соответствующее окно по роли
                OpenRoleWindow(user);
            }
            catch (System.Exception ex)
            {
                //показываем ошибку подключения к БД
                ShowError($"Ошибка подключения к БД: {ex.Message}");
            }
        }

        //перевод: валидация учетных данных
        private bool ValidateCredentials()
        {
            //получаем логин и пароль из полей
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            //проверяем на пустоту логина
            if (string.IsNullOrEmpty(login))
            {
                ShowError("Введите логин");
                return false;
            }

            //проверяем на пустоту пароля
            if (string.IsNullOrEmpty(password))
            {
                ShowError("Введите пароль");
                return false;
            }

            //если всё ок — возвращаем true
            return true;
        }

        //перевод: получение пользователя из базы данных
        private User GetUserFromDatabase()
        {
            //создаём контекст и ищем пользователя по логину с ролью
            using (var db = new EcoBumagaDbContext())
            {
                return db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == txtLogin.Text.Trim());
            }
        }

        //перевод: проверка пароля
        private bool VerifyPassword(User user)
        {
            //сравниваем хэш пароля с введённым (в реальности использовать хэширование)
            return user.PasswordHash == txtPassword.Password;
        }

        //перевод: открытие окна по роли
        private void OpenRoleWindow(User user)
        {
            //получаем имя роли или "Неизвестно" если null
            string roleName = user.Role?.RoleName ?? "Неизвестно";

            //выбираем окно в зависимости от роли
            switch (roleName)
            {
                case "Администратор": OpenWindow(new AdminWindow(user)); break;
                case "Оператор": OpenWindow(new OperatorWindow(user)); break;
                case "Менеджер": OpenWindow(new ManagerWindow(user)); break;
                case "Диспетчер": OpenWindow(new DispatcherWindow(user)); break;
                case "Водитель": OpenWindow(new DriverWindow(user)); break;
                case "Приёмщик": OpenWindow(new ReceiverWindow(user)); break;
                case "Сортировщик": OpenWindow(new SorterWindow(user)); break;
                case "Кладовщик": OpenWindow(new StorekeeperWindow(user)); break;
                case "Бухгалтер": OpenWindow(new AccountantWindow(user)); break;
                case "Директор": OpenWindow(new DirectorWindow(user)); break;
                case "Клиент": OpenWindow(new ClientWindow(user)); break;
                default: MessageBox.Show($"Неизвестная роль: {roleName}"); break;
            }

            //закрываем окно входа
            this.Close();
        }

        //перевод: открытие нового окна
        private void OpenWindow(Window window)
        {
            //показываем выбранное окно
            window.Show();
        }

        //перевод: открытие гостевого окна
        private void OpenGuestWindow()
        {
            //создаём гостевого пользователя и открываем окно
            var guestUser = CreateGuestUser();
            var guestWindow = new GuestWindow(guestUser);
            guestWindow.Show();

            //закрываем окно входа
            this.Close();
        }

        //перевод: создание гостевого пользователя
        private User CreateGuestUser()
        {
            //формируем объект гостя с базовыми данными
            return new User
            {
                UserId = 0,
                FirstName = "Гость",
                LastName = "",
                Role = new Role { RoleName = "Гость" }
            };
        }

        //перевод: отображение ошибки
        private void ShowError(string message)
        {
            //показываем текст ошибки в интерфейсе
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}