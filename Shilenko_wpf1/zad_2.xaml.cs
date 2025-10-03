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
    /// Логика взаимодействия для zad_2.xaml
    /// </summary>
    public partial class zad_2 : Window
    {
        public zad_2()
        {
            InitializeComponent();
        }

        private void Otvet_Click(object sender, RoutedEventArgs e)
        {
            string text = Text.Text;

            if (string.IsNullOrEmpty(text))
            {
                Result.Text = "Введите предложение!";
                return;
            }

            int vowelCount = CountVowels(text);
            Result.Text = $"Гласных букв: {vowelCount}";
        }

        private int CountVowels(string text)
        {
            int count = 0;
            string vowels = "аеёиоуыэюяaeiou";

            foreach (char c in text.ToLower())
            {
                if (vowels.Contains(c))
                {
                    count++;
                }
            }

            return count;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        

       
    }
}
