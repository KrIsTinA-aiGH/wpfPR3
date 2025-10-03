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
    /// Логика взаимодействия для zad_3.xaml
    /// </summary>
    public partial class zad_3 : Window
    {
        public zad_3()
        {
            InitializeComponent();
        }

        private void Otvet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                int size = int.Parse(InputBox.Text);

                if (size <= 0)
                {
                    ResultBox.Text = "Размер должен быть больше 0!";
                    return;
                }

                
                Random random = new Random();
                int[] originalNumbers = new int[size];  

                for (int i = 0; i < size; i++)
                {
                    originalNumbers[i] = random.Next(1, 21);
                }

              
                OriginalBox.Text = string.Join(" ", originalNumbers);

             
                int[] transformedNumbers = (int[])originalNumbers.Clone();

                
                if (transformedNumbers.Length > 2)
                {
                    int last = transformedNumbers[transformedNumbers.Length - 1];

                    for (int i = 1; i < transformedNumbers.Length - 1; i++)
                    {
                        if (transformedNumbers[i] % 2 == 0)
                        {
                            transformedNumbers[i] += last;  
                        }
                    }
                }

               
                ResultBox.Text = string.Join(" ", transformedNumbers);
            }
            catch
            {
                ResultBox.Text = "Ошибка! Введите целое число";
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

