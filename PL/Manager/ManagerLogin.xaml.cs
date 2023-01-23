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

        public ManagerLogin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// open products window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductViewButton_Click(object sender, RoutedEventArgs e)
        {
            Products products = new(bl);
            products.Show();
        }
        /// <summary>
        /// open orders window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Orders orders = new(bl);
            orders.Show();
        }
    }
}
