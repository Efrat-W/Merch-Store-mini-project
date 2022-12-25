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
        int password = 123;
        public ManagerLogin()
        {
            InitializeComponent();
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(PasswordTB.Text) == password)//to be continue..
                return;
        }
    }
}
