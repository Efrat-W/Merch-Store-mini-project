
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Products products = new (bl);
            products.Show();
        }

        

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Opacity = 0;
        }

        private void Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuFrame.Opacity = 1;
            MenuFrame.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }

        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
          
        }
    }
}
