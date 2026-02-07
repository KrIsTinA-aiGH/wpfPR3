using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EcoBumagaApp
{
    public partial class OperatorWindow : Window
    {
        private User _currentUser;

        //перевод: конструктор окна оператора
        public OperatorWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeWindow();
        }

        //перевод: инициализация окна
        private void InitializeWindow()
        {
            //устанавливаем заголовок с ФИО оператора
            this.Title = $"Оператор: {_currentUser.FirstName} {_currentUser.LastName}";

            //загружаем статусы и заявки
            LoadStatuses();
            LoadApplications();
        }

        //перевод: обработчик изменения фильтра статусов
        private void CbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //перезагружаем заявки с учётом выбранного фильтра
            LoadApplications();
        }

        //перевод: обработчик добавления заявки
        private void BtnAddApplication_Click(object sender, RoutedEventArgs e)
        {
            ShowApplicationEditWindow();
        }

        //перевод: обработчик редактирования заявки
        private void BtnEditApplication_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedApplication();
        }

        //перевод: обработчик удаления заявки
        private void BtnDeleteApplication_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedApplication();
        }

        //перевод: обработчик выхода
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }

        //перевод: загрузка статусов
        private void LoadStatuses()
        {
            //пробуем загрузить статусы из БД
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var statuses = dbContext.ApplicationStatuses.ToList();
                    PopulateStatusComboBox(statuses);
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку загрузки
                ShowErrorMessage("Ошибка загрузки статусов", ex.Message);
            }
        }

        //перевод: заполнение комбо статусами
        private void PopulateStatusComboBox(System.Collections.Generic.List<ApplicationStatus> statuses)
        {
            //очищаем комбо
            cbFilterStatus.Items.Clear();

            //добавляем опцию "Все статусы"
            var allStatusesItem = new ComboBoxItem { Content = "Все статусы" };
            cbFilterStatus.Items.Add(allStatusesItem);

            //добавляем статусы из списка
            foreach (var status in statuses)
            {
                var item = new ComboBoxItem
                {
                    Content = status.StatusName,
                    Tag = status
                };
                cbFilterStatus.Items.Add(item);
            }

            //выбираем первый элемент
            cbFilterStatus.SelectedIndex = 0;
        }

        //перевод: загрузка заявок
        private void LoadApplications()
        {
            //пробуем загрузить заявки с связанными данными
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var applications = dbContext.Applications
                        .Include(a => a.ClientUser)
                        .Include(a => a.SourceType)
                        .Include(a => a.Status)
                        .Include(a => a.ReceptionPoint)
                        .Include(a => a.Unit)
                        .ToList();

                    //фильтруем по статусу если выбран
                    var selectedStatus = (cbFilterStatus.SelectedItem as ComboBoxItem)?.Tag as ApplicationStatus;
                    if (selectedStatus != null)
                    {
                        applications = applications.Where(a => a.StatusId == selectedStatus.StatusId).ToList();
                    }

                    //устанавливаем источник для таблицы
                    dgApplications.ItemsSource = applications;
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку загрузки
                ShowErrorMessage("Ошибка загрузки заявок", ex.Message);
            }
        }

        //перевод: отображение окна редактирования заявки
        private void ShowApplicationEditWindow(Models.Application application = null)
        {
            //создаём окно для новой или существующей заявки
            var editWindow = application == null ? new ApplicationEditWindow() : new ApplicationEditWindow(application);

            //показываем диалог и перезагружаем если сохранено
            bool? result = editWindow.ShowDialog();
            if (result == true)
            {
                LoadApplications();
            }
        }

        //перевод: редактирование выбранной заявки
        private void EditSelectedApplication()
        {
            //проверяем выбран ли элемент
            if (dgApplications.SelectedItem == null)
            {
                ShowWarningMessage("Выберите заявку для редактирования");
                return;
            }

            //получаем выбранную заявку
            var selectedApplication = dgApplications.SelectedItem as Models.Application;

            //открываем редактирование
            ShowApplicationEditWindow(selectedApplication);
        }

        //перевод: удаление выбранной заявки
        private void DeleteSelectedApplication()
        {
            //проверяем выбран ли элемент
            if (dgApplications.SelectedItem == null)
            {
                ShowWarningMessage("Выберите заявку для удаления");
                return;
            }

            var selectedApplication = dgApplications.SelectedItem as Models.Application;

            //подтверждаем удаление
            if (ConfirmApplicationDeletion(selectedApplication))
            {
                DeleteApplicationFromDatabase(selectedApplication.ApplicationId);
            }
        }

        //перевод: подтверждение удаления заявки
        private bool ConfirmApplicationDeletion(Models.Application application)
        {
            //формируем сообщение с деталями заявки
            var message = $"Вы уверены, что хотите удалить заявку №{application.ApplicationId}?\n" +
                          $"Клиент: {application.ClientUser?.LastName} {application.ClientUser?.FirstName}";

            //возвращаем true если подтверждено
            return MessageBox.Show(message, "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: удаление заявки из базы данных
        private void DeleteApplicationFromDatabase(int applicationId)
        {
            //пробуем удалить
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    var applicationToDelete = dbContext.Applications
                        .FirstOrDefault(a => a.ApplicationId == applicationId);

                    if (applicationToDelete == null) return;

                    dbContext.Applications.Remove(applicationToDelete);
                    dbContext.SaveChanges();

                    //показываем успех и перезагружаем
                    ShowSuccessMessage("Заявка успешно удалена");
                    LoadApplications();
                }
            }
            catch (System.Exception ex)
            {
                //показываем ошибку
                ShowErrorMessage("Ошибка при удалении", ex.Message);
            }
        }

        //перевод: выход пользователя
        private void LogoutUser()
        {
            //если подтверждено — переходим к логину
            if (ConfirmLogout())
            {
                ReturnToLoginWindow();
            }
        }

        //перевод: подтверждение выхода
        private bool ConfirmLogout()
        {
            //показываем диалог
            return MessageBox.Show("Выйти из системы?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        //перевод: возврат к окну входа
        private void ReturnToLoginWindow()
        {
            //открываем логин и закрываем текущее
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        //перевод: показ ошибки
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show($"Ошибка: {message}", title,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //перевод: показ предупреждения
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Внимание",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //перевод: показ успеха
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}