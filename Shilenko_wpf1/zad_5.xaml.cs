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
    /// Логика взаимодействия для zad_5.xaml
    /// </summary>
    public partial class zad_5 : Window
    {

        public zad_5()
        {
            InitializeComponent();
        }

        private void Otvet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(rn.Text, out int n) || n <= 0)
                {
                    ResultBox.Text = "Ошибка: введите корректное число строк";
                    return;
                }

                if (!int.TryParse(rm.Text, out int m) || m <= 0)
                {
                    ResultBox.Text = "Ошибка: введите корректное число столбцов";
                    return;
                }          
                Random rand = new Random();
                int[] numbers = new int[n * m];

                
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = rand.Next(-10, 11);
                }

                
                OriginalBox.Text = string.Join(" ", numbers);

                
                Array.Sort(numbers);
                AscBox.Text = string.Join(" ", numbers);

                
                Array.Reverse(numbers);
                DescBox.Text = string.Join(" ", numbers);

                
                int min = numbers.Min();
                int max = numbers.Max();
                ResultBox.Text = $"Min: {min}   Max: {max}";
            }
            catch
            {
                ResultBox.Text = "Ошибка! Введите числа";
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
 
