
using PL.Manager;
using PL.OrderProcess;
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
        public static BO.Cart cart = new BO.Cart();
        public static Frame mainFrame;
        public MainWindow()
        {
            InitializeComponent();
            mainFrame = MainFrame;
        }


        private void NewCollBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.category category = BO.category.Notebooks;
            Catalouge catalouge = new Catalouge(bl);
            catalouge.CategorySelector.SelectedItem = category;
            catalouge.Show();
        }

        

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Opacity = 0;
        }

        private void MenuBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuFrame.Opacity = 0.9;
            MenuFrame.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }

        private void MenuFrame_MouseLeave(object sender, RoutedEventArgs e)
        {
            MenuFrame.Opacity = 0;
        }

        private void CartFrame_MouseLeave(object sender, RoutedEventArgs e)
        {
            CartFrame.Opacity = 0;
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            CartFrame.Opacity = 1;
            CartFrame.Navigate(new Uri("Cart.xaml", UriKind.Relative));
        }
       
    }
}
