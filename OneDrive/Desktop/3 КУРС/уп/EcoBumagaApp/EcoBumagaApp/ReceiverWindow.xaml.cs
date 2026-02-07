using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class ReceiverWindow : Window
    {
        private readonly User _currentUser;

        // Конструктор окна приёмщика
        public ReceiverWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();          // сразу настраиваем внешний вид окна
        }

        // Настраивает заголовок окна и отображаемое имя приёмщика
        // Инициализация окна
        private void InitializeWindow()
        {
            Title = $"Приёмщик: {_currentUser.FirstName} {_currentUser.LastName}";
            txtReceiverName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        // Обработчик нажатия кнопки "Выйти"
        // Обработка нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            TryLogout();
        }

        // Пытается выйти из системы с подтверждением
        // Метод выхода пользователя из системы
        private void TryLogout()
        {
            if (!AskUserToConfirmLogout()) return;

            OpenLoginAndCloseThisWindow();
        }

        // Показывает диалоговое окно подтверждения выхода
        // Подтверждение выхода пользователя
        private bool AskUserToConfirmLogout()
        {
            var result = MessageBox.Show(
                "Завершить смену и выйти?",
                "Выход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        // Открывает окно входа и закрывает текущее окно приёмщика
        // Возврат к окну авторизации
        private void OpenLoginAndCloseThisWindow()
        {
            new LoginWindow().Show();
            Close();
        }
    }
}