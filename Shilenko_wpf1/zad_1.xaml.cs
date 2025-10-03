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
    /// Логика взаимодействия для zad_1.xaml
    /// </summary>
    public partial class zad_1 : Window
    {
        public zad_1()
        {
            InitializeComponent();
        }

        private void Otvet_Click(object sender, RoutedEventArgs e)
        {
            
            string input = Number.Text;

           
            if (int.TryParse(input, out int number))
            {
               
                string chetnoe = (number % 2 == 0) ? "четное" : "нечетное";

                
                string digits;
                int length = number.ToString().Length;

                if (length == 1) digits = "однозначное";
                else if (length == 2) digits = "двузначное";
                else if (length == 3) digits = "трехзначное";
                else if (length == 4) digits = "четырехзначное";
                else digits = "многозначное";

                
                Result.Text = $"{chetnoe} {digits} число";
            }
            else
            {
                Result.Text = "Введите число!";
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

