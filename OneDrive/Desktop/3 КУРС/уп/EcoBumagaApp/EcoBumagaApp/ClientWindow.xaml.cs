using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class ClientWindow : Window
    {
        private User _currentUser;
        private ClientProfile _clientProfile; //предполагаем, что используется, хотя в коде не видно

        //перевод: конструктор окна клиента
        public ClientWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок и текст с ФИО
            this.Title = $"Клиент: {_currentUser.FirstName} {_currentUser.LastName}";
            txtClientName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя
        private void LogoutUser()
        {
            //подтверждаем и переходим
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

        //перевод: возврат к логину
        private void ReturnToLoginWindow()
        {
            //открываем логин и закрываем текущее
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}