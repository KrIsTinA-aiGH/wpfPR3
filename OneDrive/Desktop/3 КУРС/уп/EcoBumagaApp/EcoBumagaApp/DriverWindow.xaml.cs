using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class DriverWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна водителя-экспедитора
        public DriverWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок с полной должностью и ФИО
            this.Title = $"Водитель-экспедитор: {_currentUser.FirstName} {_currentUser.LastName}";

            //отображаем ФИО водителя в текстовом поле
            txtDriverName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход пользователя из системы
        private void LogoutUser()
        {
            //если водитель подтвердил завершение дня — закрываем окно
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение завершения рабочего дня
        private bool ConfirmLogout()
        {
            //специфичный текст для водителя — завершить рабочий день
            return MessageBox.Show(
                "Завершить рабочий день и выйти?",
                "Выход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: возврат к окну входа
        private void ReturnToLoginWindow()
        {
            //открываем окно авторизации
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            //закрываем текущее окно водителя
            this.Close();
        }
    }
}