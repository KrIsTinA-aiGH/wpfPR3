using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class StorekeeperWindow : Window
    {
        private readonly User _currentUser;

        // Конструктор окна кладовщика
        public StorekeeperWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();          // сразу задаём заголовок и имя сотрудника
        }

        // Устанавливает заголовок окна и отображаемое имя кладовщика
        // Инициализация окна
        private void InitializeWindow()
        {
            Title = $"Кладовщик: {_currentUser.FirstName} {_currentUser.LastName}";
            txtStorekeeperName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
        }

        // Обработчик нажатия кнопки "Выйти"
        // Обработка нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            TryLogout();
        }

        // Выполняет попытку выхода с подтверждением пользователя
        // Метод выхода пользователя из системы
        private void TryLogout()
        {
            if (!AskUserToConfirmLogout()) return;

            OpenLoginAndCloseThisWindow();
        }

        // Показывает диалог подтверждения завершения смены
        // Подтверждение выхода пользователя
        private bool AskUserToConfirmLogout()
        {
            var result = MessageBox.Show(
                "Завершить смену кладовщика?",
                "Выход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        // Открывает окно авторизации и закрывает текущее окно
        // Возврат к окну авторизации
        private void OpenLoginAndCloseThisWindow()
        {
            new LoginWindow().Show();
            Close();
        }
    }
}