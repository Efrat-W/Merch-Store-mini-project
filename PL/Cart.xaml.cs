using BO;
using PL.OrderProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public Cart()
        {
            InitializeComponent();
            ProductsScrollView.DataContext = MainWindow.cart.Items;
            MainGrid.DataContext = MainWindow.cart;
        }
        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((OrderItem)ProductsScrollView.SelectedItem).ProductId;
            new ViewProduct(id).ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.mainFrame.CanGoBack)
                MainWindow.mainFrame.RemoveBackEntry();
            
            MainWindow.mainFrame.Navigate(new Uri("OrderMaking.xaml", UriKind.Relative));
           
        }
        private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderItem item = (BO.OrderItem)((Button)sender).DataContext;
            MainWindow.cart = bl.Cart.UpdateProductAmount(MainWindow.cart, item.ProductId, 1);
            ProductsScrollView.Items.Refresh();
            Total.Content = MainWindow.cart.TotalPrice;

        }
        private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderItem item = (BO.OrderItem)((Button)sender).DataContext;
            MainWindow.cart = bl.Cart.UpdateProductAmount(MainWindow.cart, item.ProductId, -1);
            ProductsScrollView.Items.Refresh();
            Total.Content = MainWindow.cart.TotalPrice;
        }
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderItem item = (BO.OrderItem)((Button)sender).DataContext;
            MainWindow.cart = bl.Cart.UpdateProductAmount(MainWindow.cart, item.ProductId, 0);
            ProductsScrollView.Items.Refresh();
            Total.Content = MainWindow.cart.TotalPrice;
        }
    }
}
