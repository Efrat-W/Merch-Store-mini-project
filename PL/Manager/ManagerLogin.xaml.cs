using DO;
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

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ManagerLogin.xaml
    /// </summary>
    public partial class ManagerLogin : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        const string PASSWORD = "123";
        string currentPassword = "";
        
        public string password
        {
            get { return (string)GetValue(passwordProperty); }
            set { SetValue(passwordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty passwordProperty =
            DependencyProperty.Register("password", typeof(string), typeof(ManagerLogin));


        public ManagerLogin()
        {
            InitializeComponent();
        }

        private void ProductViewButton_Click(object sender, RoutedEventArgs e)
        {
            Products products = new(bl);
            products.Show();
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Orders orders = new(bl);
            orders.Show();
        }

        //private void logOutBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Login.Visibility = Visibility.Visible;
        //    LoggedIn.Visibility = Visibility.Hidden;
        //}
    }
}
