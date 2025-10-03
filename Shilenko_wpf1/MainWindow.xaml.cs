
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
using System.Windows.Navigation;
using Shilenko_wpf1.Pages;
using System.Windows.Shapes; 

namespace Shilenko_wpf1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Autho());
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }
        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                zad1.Visibility = Visibility.Visible;
                zad2.Visibility = Visibility.Visible;
                zad3.Visibility = Visibility.Visible;
                zad4.Visibility = Visibility.Visible;
                zad5.Visibility = Visibility.Visible;
 
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                zad1.Visibility = Visibility.Hidden;
                zad2.Visibility = Visibility.Hidden;
                zad3.Visibility = Visibility.Hidden;
                zad4.Visibility = Visibility.Hidden;
                zad5.Visibility = Visibility.Hidden;

                btnBack.Visibility = Visibility.Hidden;
            }
        }

        public void Switching(Window win)
        {
            win.Show();

            this.Close();
        }
        private void Zad1_Click(object sender, RoutedEventArgs e)
        {

            zad_1 zadanie = new zad_1();
            Switching(zadanie);


        }

        private void Zad2_Click(object sender, RoutedEventArgs e)
        {
            zad_2 zadanie = new zad_2();
            Switching(zadanie);
        }

        private void Zad3_Click(object sender, RoutedEventArgs e)
        {
            zad_3 zadanie = new zad_3();
            Switching(zadanie);
        }

        private void Zad4_Click(object sender, RoutedEventArgs e)
        {
            zad_4 zadanie = new zad_4();
            Switching(zadanie);
        }

        private void Zad5_Click(object sender, RoutedEventArgs e)
        {
            zad_5 zadanie = new zad_5();
            Switching(zadanie);
        }

        private void Main_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void TextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void FrmMain_Navigated(object sender, NavigationEventArgs e)
        {

        }

        

        
    }
}
