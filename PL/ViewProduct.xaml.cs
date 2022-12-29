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

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        int ID;
        public ViewProduct(int id)
        {
            ID=id;
            InitializeComponent();
            MainGrid.DataContext = bl.Product.RequestByIdManager(id);
        }
        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Opacity = 0;
        }

        private void MenuBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuFrame.Opacity = 0.9;
            MenuFrame.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }

        private void MenuFrame_MouseLeave(object sender, RoutedEventArgs e)
        {
            MenuFrame.Opacity = 0;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            bl.Cart.AddProduct(MainWindow.cart , ID);
        }
    }
}
