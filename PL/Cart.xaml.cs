using BO;
using PL.OrderProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static BO.Cart cart;
        public static readonly DependencyProperty ItemsDependency =
       DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<OrderItem?>), typeof(Catalog));
        public ObservableCollection<OrderItem?> Items
        {
            get => (ObservableCollection<OrderItem?>)GetValue(ItemsDependency);
            private set => SetValue(ItemsDependency, value);
        }
        public Cart(BO.Cart cart1)
        {
            cart = cart1;
            if (cart.Items != null)
                Items = new(cart.Items);
            else
                Items = null;
            InitializeComponent();
        }
        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((OrderItem)ProductsScrollView.SelectedItem).ProductId;
            MainWindow.mainFrame.Navigate(new ViewProduct(id,cart));
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
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, 1);
            Items = new(cart.Items);
            Total.Content = cart.TotalPrice;

        }
        private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderItem item = (BO.OrderItem)((Button)sender).DataContext;
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, -1);
            Items = new(cart.Items);
            Total.Content = cart.TotalPrice;
        }
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.OrderItem item = (BO.OrderItem)((Button)sender).DataContext;
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, 0);
            Items = new(cart.Items); ;
            Total.Content = cart.TotalPrice;
        }
    }
}
