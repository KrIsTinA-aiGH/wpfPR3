using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EcoBumagaApp
{
    public partial class ApplicationEditWindow : Window
    {
        private Models.Application _applicationToEdit;
        private bool _isEditMode = false;

        //перевод: конструктор для новой заявки
        public ApplicationEditWindow()
        {
            InitializeComponent();
            LoadData();
            this.Title = "Новая заявка";
        }

        //перевод: конструктор для редактирования заявки
        public ApplicationEditWindow(Models.Application application) : this()
        {
            _applicationToEdit = application;
            _isEditMode = true;
            this.Title = "Редактирование заявки";
            LoadApplicationData();
        }

        //перевод: загрузка данных для формы
        private void LoadData()
        {
            //пробуем загрузить все необходимые списки
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    LoadClients(dbContext);
                    LoadSourceTypes(dbContext);
                    LoadReceptionPoints(dbContext);
                    LoadMeasurementUnits(dbContext);
                    LoadStatuses(dbContext);
                }
            }
            catch (Exception ex)
            {
                //показываем ошибку
                ShowErrorMessage("Ошибка загрузки данных", ex.Message);
            }
        }

        //перевод: загрузка клиентов
        private void LoadClients(EcoBumagaDbContext dbContext)
        {
            //фильтруем пользователей с ролью клиент
            var clients = dbContext.Users.Where(u => u.Role.RoleName == "Клиент").ToList();
            cbClient.ItemsSource = clients;
            if (clients.Any()) cbClient.SelectedIndex = 0;
        }

        //перевод: загрузка типов источников
        private void LoadSourceTypes(EcoBumagaDbContext dbContext)
        {
            var sourceTypes = dbContext.ApplicationSourceTypes.ToList();
            cbSourceType.ItemsSource = sourceTypes;
            if (sourceTypes.Any()) cbSourceType.SelectedIndex = 0;
        }

        //перевод: загрузка пунктов приема
        private void LoadReceptionPoints(EcoBumagaDbContext dbContext)
        {
            var receptionPoints = dbContext.ReceptionPoints.ToList();
            cbReceptionPoint.ItemsSource = receptionPoints;
            if (receptionPoints.Any()) cbReceptionPoint.SelectedIndex = 0;
        }

        //перевод: загрузка единиц измерения
        private void LoadMeasurementUnits(EcoBumagaDbContext dbContext)
        {
            var units = dbContext.MeasurementUnits.ToList();
            cbUnit.ItemsSource = units;
            if (units.Any()) cbUnit.SelectedIndex = 0;
        }

        //перевод: загрузка статусов
        private void LoadStatuses(EcoBumagaDbContext dbContext)
        {
            var statuses = dbContext.ApplicationStatuses.ToList();
            cbStatus.ItemsSource = statuses;
            if (statuses.Any()) cbStatus.SelectedIndex = 0;
        }

        //перевод: загрузка данных заявки для редактирования
        private void LoadApplicationData()
        {
            //заполняем поля из существующей заявки
            if (_applicationToEdit == null) return;

            txtSourceAddress.Text = _applicationToEdit.SourceAddress;
            txtEstimatedWeight.Text = _applicationToEdit.EstimatedWeight.ToString();
            txtBonuses.Text = _applicationToEdit.BonusesAwarded.ToString();

            //выбираем соответствующие элементы в комбо
            SelectComboItem(cbClient, u => ((User)u).UserId == _applicationToEdit.UserId);
            SelectComboItem(cbSourceType, st => ((ApplicationSourceType)st).SourceTypeId == _applicationToEdit.SourceTypeId);
            SelectComboItem(cbReceptionPoint, rp => ((ReceptionPoint)rp).PointId == _applicationToEdit.ReceptionPointId);
            SelectComboItem(cbUnit, u => ((MeasurementUnit)u).UnitId == _applicationToEdit.UnitId);
            SelectComboItem(cbStatus, s => ((ApplicationStatus)s).StatusId == _applicationToEdit.StatusId);
        }

        //перевод: выбор элемента в комбо по условию
        private void SelectComboItem(ComboBox combo, Func<object, bool> predicate)
        {
            combo.SelectedItem = combo.Items.OfType<object>().FirstOrDefault(predicate);
        }

        //перевод: сохранение заявки
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //очищаем ошибки и валидируем
            ClearError();
            if (!ValidateForm()) return;

            //пробуем сохранить
            try
            {
                using (var dbContext = new EcoBumagaDbContext())
                {
                    if (_isEditMode)
                    {
                        UpdateExistingApplication(dbContext);
                    }
                    else
                    {
                        CreateNewApplication(dbContext);
                    }

                    dbContext.SaveChanges();
                    ShowSuccessMessage();
                    CloseWindowWithSuccess();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Ошибка сохранения", ex.Message);
            }
        }

        //перевод: валидация формы
        private bool ValidateForm()
        {
            //проверяем обязательные поля
            if (cbClient.SelectedItem == null) return ShowError("Выберите клиента", cbClient);
            if (cbSourceType.SelectedItem == null) return ShowError("Выберите тип источника", cbSourceType);
            if (string.IsNullOrWhiteSpace(txtSourceAddress.Text)) return ShowError("Введите адрес источника", txtSourceAddress);
            if (cbStatus.SelectedItem == null) return ShowError("Выберите статус", cbStatus);

            //проверяем числовые поля
            if (!decimal.TryParse(txtEstimatedWeight.Text, out _)) return ShowError("Неверный формат веса", txtEstimatedWeight);
            if (!decimal.TryParse(txtBonuses.Text, out _)) return ShowError("Неверный формат бонусов", txtBonuses);

            return true;
        }

        //перевод: обновление существующей заявки
        private void UpdateExistingApplication(EcoBumagaDbContext dbContext)
        {
            //находим и обновляем свойства
            var application = dbContext.Applications.FirstOrDefault(a => a.ApplicationId == _applicationToEdit.ApplicationId);
            if (application == null) throw new Exception("Заявка не найдена");

            UpdateApplicationProperties(application);
        }

        //перевод: создание новой заявки
        private void CreateNewApplication(EcoBumagaDbContext dbContext)
        {
            //создаём новый объект и добавляем
            var selectedClient = cbClient.SelectedItem as User;
            var selectedSourceType = cbSourceType.SelectedItem as ApplicationSourceType;
            var selectedStatus = cbStatus.SelectedItem as ApplicationStatus;
            var selectedReceptionPoint = cbReceptionPoint.SelectedItem as ReceptionPoint;
            var selectedUnit = cbUnit.SelectedItem as MeasurementUnit;

            var newApplication = CreateApplicationObject(selectedClient, selectedSourceType, selectedStatus, selectedReceptionPoint, selectedUnit);
            dbContext.Applications.Add(newApplication);
        }

        //перевод: обновление свойств заявки
        private void UpdateApplicationProperties(Models.Application application)
        {
            //обновляем поля из формы
            application.SourceAddress = txtSourceAddress.Text.Trim();
            application.EstimatedWeight = decimal.Parse(txtEstimatedWeight.Text);
            application.BonusesAwarded = decimal.Parse(txtBonuses.Text);
            application.ReceptionPointId = (cbReceptionPoint.SelectedItem as ReceptionPoint)?.PointId;
            application.UnitId = (cbUnit.SelectedItem as MeasurementUnit)?.UnitId;
            application.StatusId = (cbStatus.SelectedItem as ApplicationStatus).StatusId;
            //другие поля если нужно
        }

        //перевод: создание объекта заявки
        private Models.Application CreateApplicationObject(User client, ApplicationSourceType sourceType, ApplicationStatus status, ReceptionPoint receptionPoint, MeasurementUnit unit)
        {
            //создаём новый с текущими данными
            var application = new Models.Application
            {
                UserId = client.UserId,
                CreatedByEmployeeId = null,
                SourceTypeId = sourceType.SourceTypeId,
                SourceAddress = txtSourceAddress.Text.Trim(),
                ReceptionPointId = receptionPoint?.PointId,
                CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                StatusId = status.StatusId,
                BonusesAwarded = decimal.Parse(txtBonuses.Text)
            };

            if (decimal.TryParse(txtEstimatedWeight.Text, out decimal weight))
                application.EstimatedWeight = weight;

            if (unit != null)
                application.UnitId = unit.UnitId;

            return application;
        }

        //перевод: отмена
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowWithCancel();
        }

        //перевод: закрытие с успехом
        private void CloseWindowWithSuccess()
        {
            this.DialogResult = true;
            this.Close();
        }

        //перевод: закрытие с отменой
        private void CloseWindowWithCancel()
        {
            this.DialogResult = false;
            this.Close();
        }

        //перевод: показ успеха
        private void ShowSuccessMessage()
        {
            MessageBox.Show(_isEditMode ? "Заявка успешно обновлена" : "Заявка успешно создана",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //перевод: показ ошибки
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show($"Ошибка: {message}", title,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //перевод: показ ошибки в форме
        private bool ShowError(string message, System.Windows.Controls.Control control = null)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
            if (control != null) control.Focus();
            return false;
        }

        //перевод: очистка ошибки
        private void ClearError()
        {
            txtError.Text = string.Empty;
            txtError.Visibility = Visibility.Collapsed;
        }
    }
}