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
using System.Windows.Shapes;

namespace PL.OrderProcess
{
    /// <summary>
    /// Interaction logic for OrderMaking.xaml
    /// </summary>
    public partial class OrderMaking : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public OrderMaking()
        {
            InitializeComponent();
            ProductsScrollView.DataContext = MainWindow.cart.Items;
            MainGrid.DataContext = MainWindow.cart;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.cart.CustomerName=NameTB.Text;
            MainWindow.cart.CustomerEmail= EmailTB.Text;
            MainWindow.cart.CustomerAddress = AdressTB.Text;
            BO.Order order=bl.Cart.Approve(MainWindow.cart);
            MessageBox.Show("HOORAY! Your order has been received in the system and will start its way to you soon, we cant wait!" +
                "Your order id is: "+ order.Id);
        }
    }
}
