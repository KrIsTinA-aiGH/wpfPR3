using EcoBumagaApp.Data;
using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EcoBumagaApp
{
    public partial class EmployeeEditWindow : Window
    {
        private Employee? _employeeToEdit;
        private bool _isEditMode;

        // Конструктор для добавления нового сотрудника
        public EmployeeEditWindow()
        {
            InitializeComponent();
            _isEditMode = false;
            Title = "Добавление сотрудника";
            LoadData();  // загружаем списки для добавления
        }

        // Конструктор для редактирования существующего сотрудника
        public EmployeeEditWindow(Employee employee) : this()
        {
            _employeeToEdit = employee;
            _isEditMode = true;
            Title = "Редактирование сотрудника";
            LoadData();  // перезагружаем списки с учётом режима редактирования (все пользователи)
            LoadEmployeeData();  // заполняем поля данными сотрудника
        }

        // Загружает списки пользователей и должностей в ComboBox
        // Загрузка данных для формы
        private void LoadData()
        {
            try
            {
                using var db = new EcoBumagaDbContext();

                // Пользователи: все в режиме редактирования, иначе только не-сотрудники
                var users = _isEditMode
                    ? db.Users.ToList()
                    : db.Users.Where(u => !db.Employees.Any(e => e.UserId == u.UserId)).ToList();
                cbUser.ItemsSource = users;
                if (users.Count > 0) cbUser.SelectedIndex = 0;

                // Должности: все всегда
                var positions = db.EmployeePositions.ToList();
                cbPosition.ItemsSource = positions;
                if (positions.Count > 0) cbPosition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Ошибка загрузки", ex.Message);
            }
        }

        // Загружает данные редактируемого сотрудника в поля формы
        // Загрузка данных сотрудника
        private void LoadEmployeeData()
        {
            if (_employeeToEdit == null) return;

            try
            {
                using var db = new EcoBumagaDbContext();
                var employee = db.Employees
                    .Include(e => e.User)
                    .Include(e => e.Position)
                    .FirstOrDefault(e => e.EmployeeId == _employeeToEdit.EmployeeId);

                if (employee == null)
                {
                    ShowErrorMessage("Сотрудник не найден", "Запись удалена или недоступна");
                    CloseWindowWithCancel();  // закрываем окно, если данные не загрузились
                    return;
                }

                // Заполняем поля
                cbUser.SelectedItem = cbUser.Items.Cast<User>().FirstOrDefault(u => u.UserId == employee.UserId);
                cbPosition.SelectedItem = cbPosition.Items.Cast<EmployeePosition>().FirstOrDefault(p => p.PositionId == employee.PositionId);
                txtEmployeeNumber.Text = employee.EmployeeNumber;
                dpHireDate.SelectedDate = employee.HireDate?.ToLocalTime();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Ошибка загрузки данных", ex.Message);
                CloseWindowWithCancel();  // закрываем окно при ошибке
            }
        }

        // Обработчик кнопки "Сохранить"
        // Сохранение сотрудника
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;
            SaveOrUpdateEmployee();
        }

        // Проверяет корректность заполнения формы
        // Валидация формы
        private bool ValidateForm()
        {
            ClearError();

            if (cbUser.SelectedItem == null)
                return ShowFieldError(cbUser, "Выберите пользователя");

            if (cbPosition.SelectedItem == null)
                return ShowFieldError(cbPosition, "Выберите должность");

            if (string.IsNullOrWhiteSpace(txtEmployeeNumber.Text))
                return ShowFieldError(txtEmployeeNumber, "Введите табельный номер");

            var user = (User)cbUser.SelectedItem!;
            var employeeNumber = txtEmployeeNumber.Text.Trim();

            // Проверка уникальности номера (исключая текущего при редактировании)
            using var db = new EcoBumagaDbContext();
            if (db.Employees.Any(e => e.EmployeeNumber == employeeNumber &&
                                      (_isEditMode ? e.EmployeeId != _employeeToEdit!.EmployeeId : true)))
                return ShowFieldError(txtEmployeeNumber, "Табельный номер уже занят");

            // Проверка: пользователь не должен быть уже сотрудником (только при добавлении)
            if (!_isEditMode && db.Employees.Any(e => e.UserId == user.UserId))
                return ShowFieldError(cbUser, "Этот пользователь уже является сотрудником");

            return true;
        }

        // Создаёт или обновляет сотрудника в базе
        // Сохранение / обновление сотрудника
        private void SaveOrUpdateEmployee()
        {
            try
            {
                using var db = new EcoBumagaDbContext();
                var user = (User)cbUser.SelectedItem!;
                var position = (EmployeePosition)cbPosition.SelectedItem!;
                var hireDate = dpHireDate.SelectedDate?.Date.ToUniversalTime();  // конвертируем в UTC

                if (_isEditMode)
                {
                    var employee = db.Employees.FirstOrDefault(e => e.EmployeeId == _employeeToEdit!.EmployeeId);
                    if (employee == null)
                    {
                        ShowErrorMessage("Сотрудник не найден", "Запись удалена");
                        return;
                    }

                    employee.UserId = user.UserId;
                    employee.PositionId = position.PositionId;
                    employee.EmployeeNumber = txtEmployeeNumber.Text.Trim();
                    employee.HireDate = hireDate;
                }
                else
                {
                    var newEmployee = new Employee
                    {
                        UserId = user.UserId,
                        PositionId = position.PositionId,
                        EmployeeNumber = txtEmployeeNumber.Text.Trim(),
                        HireDate = hireDate
                    };
                    db.Employees.Add(newEmployee);
                }

                db.SaveChanges();
                ShowSuccessAndClose();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Ошибка сохранения", ex.Message);
            }
        }

        // Показывает сообщение об успехе и закрывает окно
        // Успешное завершение
        private void ShowSuccessAndClose()
        {
            MessageBox.Show(
                _isEditMode ? "Сотрудник успешно обновлён" : "Сотрудник успешно добавлен",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        // Обработчик кнопки "Отмена"
        // Отмена и закрытие окна
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowWithCancel();
        }

        // Закрывает окно с отменой
        // Закрытие с отменой
        private void CloseWindowWithCancel()
        {
            DialogResult = false;
            Close();
        }

        // Показывает ошибку и фокусируется на поле
        private bool ShowFieldError(Control control, string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
            control.Focus();
            return false;
        }

        // Очищает сообщение об ошибке
        // Очистка ошибки
        private void ClearError()
        {
            txtError.Text = "";
            txtError.Visibility = Visibility.Collapsed;
        }

        // Показывает всплывающее сообщение об ошибке
        // Показ системной ошибки
        private void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show($"Ошибка: {message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}