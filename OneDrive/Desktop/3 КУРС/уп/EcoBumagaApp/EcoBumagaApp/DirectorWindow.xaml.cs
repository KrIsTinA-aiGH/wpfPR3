using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class DirectorWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна директора
        public DirectorWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок окна с ФИО директора
            this.Title = $"Директор: {_currentUser.FirstName} {_currentUser.LastName}";

            //отображаем ФИО в текстовом поле
            txtDirectorName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя из системы
        private void LogoutUser()
        {
            //если пользователь подтвердил выход — переходим к окну логина
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение выхода
        private bool ConfirmLogout()
        {
            //показываем диалог подтверждения и возвращаем результат
            return MessageBox.Show(
                "Выйти из системы?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: возврат к окну входа
        private void ReturnToLoginWindow()
        {
            //открываем окно авторизации
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            //закрываем текущее окно директора
            this.Close();
        }
    }
}