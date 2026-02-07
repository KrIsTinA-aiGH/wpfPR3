using EcoBumagaApp.Models;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class GuestWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна гостя
        public GuestWindow(User user = null)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //если пользователь передан — показываем его данные
            if (_currentUser != null)
            {
                this.Title = $"Гость: {_currentUser.FirstName} {_currentUser.LastName}";
                txtGuestName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";
            }
            //иначе — режим неавторизованного гостя
            else
            {
                this.Title = "ЭкоБумага - Гость";
                txtGuestName.Text = "Неавторизованный пользователь";
            }
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: выход из гостевого режима
        private void LogoutUser()
        {
            //если подтверждено — возвращаемся к окну входа
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение выхода
        private bool ConfirmLogout()
        {
            //показываем стандартный диалог подтверждения
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

            //закрываем текущее гостевое окно
            this.Close();
        }
    }
}