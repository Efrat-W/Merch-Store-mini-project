using BlImplementation;
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
        private BO.Cart cart;
        public OrderMaking(BO.Cart cart)
        {
            InitializeComponent();
            ProductsScrollView.DataContext = cart.Items;
            MainGrid.DataContext = cart;
            this.cart = cart;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cart.CustomerName=NameTB.Text;
            cart.CustomerEmail= EmailTB.Text;
            cart.CustomerAddress = AdressTB.Text;
            BO.Order order = new();
            try
            {
                order = bl.Cart.Approve(cart);
            }
            catch (InvalidArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("HOORAY! Your order has been received in the system and will start its way to you soon, we cant wait!" +
                " Your order id is: "+ order.Id+ ". An approval mail is  already in your inbox.");
        }
    }
}
