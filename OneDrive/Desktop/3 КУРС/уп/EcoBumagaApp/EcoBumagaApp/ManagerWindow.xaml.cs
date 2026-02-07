using EcoBumagaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EcoBumagaApp
{
    public partial class ManagerWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна менеджера
        public ManagerWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок с ФИО менеджера
            this.Title = $"Менеджер: {_currentUser.FirstName} {_currentUser.LastName}";

            //отображаем ФИО в текстовом поле
            txtManagerName.Text = $"{_currentUser.LastName} {_currentUser.FirstName}";

            //показываем версию и текущую дату
            txtVersion.Text = $"Версия 1.0 • {DateTime.Now:dd.MM.yyyy}";

            //загружаем тестовые данные
            LoadSampleData();
        }

        //перевод: обработчик нажатия кнопки выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: обработчик выбора клиента в таблице
        private void dgClients_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //здесь можно добавить логику обработки выбора клиента
        }

        //перевод: загрузка тестовых данных
        private void LoadSampleData()
        {
            dgClients.ItemsSource = new[]
            {
        new { ФИО = "Иванов Иван Иванович", Организация = "ООО 'Ромашка'", Телефон = "+7 (999) 123-45-67", Бонусы = 1500.50m, Заявок = 12, Статус = "Активный" },
        new { ФИО = "Петрова Мария Сергеевна", Организация = "", Телефон = "+7 (999) 765-43-21", Бонусы = 500.00m, Заявок = 5, Статус = "Активный" }
    };
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
            //показываем диалог и возвращаем true если "Да"
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

            //закрываем текущее окно менеджера
            this.Close();
        }
    }

}