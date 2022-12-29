using BO;
using PL.OrderProcess;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
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
            MainWindow.mainFrame.Navigate(new Uri("OrderMaking.xaml", UriKind.Relative)); 
          
        }
    }
}
