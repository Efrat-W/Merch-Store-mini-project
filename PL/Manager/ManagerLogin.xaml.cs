﻿using DO;
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
            currentPassword= PasswordTB.Text;
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
                if (currentPassword == PASSWORD)
                    Toggle();
            }
            else
            {
                //regex needed
                
            }
        }

        private void Toggle()
        {
            //if (Login.Visibility == Visibility.Hidden)
            //{
            //    LoggedIn.Visibility = Visibility.Hidden;
            //    Login.Visibility = Visibility.Visible;
            //}
            //else
            //{
                Login.Visibility = Visibility.Hidden;
                LoggedIn.Visibility = Visibility.Visible;
            
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

        private void PasswordTB_KeyUp(object sender, KeyEventArgs e)
        {
            char inputKey = PasswordTB.Text.ElementAt(PasswordTB.Text.Length - 1);
            currentPassword += inputKey;
            PasswordTB.Text.Replace(inputKey, '•');
        }
    }
}
