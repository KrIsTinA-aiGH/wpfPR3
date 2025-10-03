using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shilenko_wpf1
{
    /// <summary>
    /// Логика взаимодействия для zad_4.xaml
    /// </summary>
    public partial class zad_4 : Window
    {
        public zad_4()
        {
            InitializeComponent();
        }

        private void Otvet_Click(object sender, RoutedEventArgs e)
        {
            string input = InputBox.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                ResultBox.Text = "Введите числа!";
                return;
            }

            try
            {
                
                string[] numbersText = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (numbersText.Length == 0)
                {
                    ResultBox.Text = "Введите числа!";
                    return;
                }

                int[] numbers = new int[numbersText.Length];

                
                for (int i = 0; i < numbersText.Length; i++)
                {
                    numbers[i] = int.Parse(numbersText[i].Trim());
                }

                
                if (numbers.Length < 5)
                {
                    ResultBox.Text = $"Нужно минимум 5 чисел! Введено: {numbers.Length}";
                    return;
                }

                
                int temp = numbers[2];
                numbers[2] = numbers[4];
                numbers[4] = temp;

               
                ResultBox.Text = string.Join(" ", numbers);
            }
            catch (FormatException)
            {
                ResultBox.Text = "Ошибка! Вводите только целые числа";
            }
            catch (Exception ex)
            {
                ResultBox.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        
    }
 }
    

