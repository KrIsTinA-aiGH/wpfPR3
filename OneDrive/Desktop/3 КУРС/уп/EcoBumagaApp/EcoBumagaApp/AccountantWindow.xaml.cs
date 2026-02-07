using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class AccountantWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна бухгалтера
        public AccountantWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок окна с ФИО бухгалтера
            this.Title = $"Бухгалтер: {_currentUser.FirstName} {_currentUser.LastName}";

            //отображаем ФИО в текстовом поле
            txtAccountantName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя из системы
        private void LogoutUser()
        {
            //показываем диалог подтверждения выхода
            var result = MessageBox.Show(
                "Выйти из системы?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            //если пользователь подтвердил — закрываем текущее окно и открываем окно входа
            if (result == MessageBoxResult.Yes)
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: возврат к окну входа
        private void ReturnToLoginWindow()
        {
            //создаём и показываем окно логина
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            //закрываем текущее окно бухгалтера
            this.Close();
        }
    }
}