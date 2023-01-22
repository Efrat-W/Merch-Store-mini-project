
using PL.Manager;
using PL.OrderProcess;
using Simulator;
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
        private static BO.Cart cart = new();
        internal static Frame mainFrame;
        public MainWindow()
        {
            InitializeComponent();
            mainFrame = MainFrame;
            mainFrame.Navigate(new HomePage(cart));
        }
        /// <summary>
        /// open the cart page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            CartFrame.Navigate(new Cart(cart));
        }
        /// <summary>
        /// open the menu page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(new MainMenu(cart));
        }
    }
}
