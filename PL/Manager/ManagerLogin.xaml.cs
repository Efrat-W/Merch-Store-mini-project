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
        public ManagerLogin()
        {
            InitializeComponent();
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPassword = PasswordTB.Text;
            if (currentPassword == PASSWORD)
                Toggle();
            PasswordTB.Text = "";
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

        private void PasswordTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                currentPassword = PasswordTB.Text;
                if (currentPassword == PASSWORD)
                    Toggle();
            }
            else
            {
                //regex needed
                //char inputKey = PasswordTB.Text.ElementAt(PasswordTB.Text.Length - 1);
                //char inputKey = 'a';
                //if (e.Key.ToString().StartsWith("NumPad"))
                //    inputKey = e.Key.ToString().Last();
                //else if (e.Key.ToString().StartsWith("D"))
                //    inputKey = e.Key.ToString().Last();
                //else
                //    e.Handled = true;
                //currentPassword.Append(inputKey);
                //int len = PasswordTB.Text.Length+1;
                //PasswordTB.Text = "";
                //for (int i = 0; i < len; i++)
                //    PasswordTB.AppendText("•");

                //e.Handled = false;
            }
        }

        private void Toggle()
        {
            if (Login.Visibility == Visibility.Hidden)
            {
                LoggedIn.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
            }
            else
            {
                Login.Visibility = Visibility.Hidden;
                LoggedIn.Visibility = Visibility.Visible;
            }
        }

        private void logOutText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggle();
        }


        private void logOutText_MouseEnter(object sender, MouseEventArgs e)
        {
            logOutText.TextDecorations = TextDecorations.Underline;
        }

        private void logOutText_MouseLeave(object sender, MouseEventArgs e)
        {
            logOutText.TextDecorations = null;
        }
    }
}
