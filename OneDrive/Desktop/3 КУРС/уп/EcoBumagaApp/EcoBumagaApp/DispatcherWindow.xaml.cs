using EcoBumagaApp.Models;
using System;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class DispatcherWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна диспетчера
        public DispatcherWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок с ФИО диспетчера
            this.Title = $"Диспетчер: {_currentUser.FirstName} {_currentUser.LastName}";

            //отображаем ФИО в текстовом поле
            txtDispatcherName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";

            //показываем версию приложения и текущую дату
            txtVersion.Text = $"Версия 1.0 • {DateTime.Now:dd.MM.yyyy}";
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя из системы
        private void LogoutUser()
        {
            //если подтверждено — переходим к окну входа
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение выхода
        private bool ConfirmLogout()
        {
            //показываем диалог и возвращаем true если выбрано "Да"
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

            //закрываем текущее окно диспетчера
            this.Close();
        }
    }
}