using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class SorterWindow : Window
    {
        private readonly User _currentUser;

        // Конструктор окна сортировщика
        public SorterWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();          // сразу задаём заголовок и имя
        }

        // Устанавливает заголовок окна и отображаемое имя сортировщика
        // Инициализация окна
        private void InitializeWindow()
        {
            Title = $"Сортировщик: {_currentUser.FirstName} {_currentUser.LastName}";
            txtSorterName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
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
                "Завершить смену сортировщика?",
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