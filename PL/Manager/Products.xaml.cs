using BlApi;
using BlImplementation;
using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        private IBl bl = new Bl();
        public Products(IBl bl1)
        {
            bl = bl1;
            InitializeComponent();
            ProductsListView.ItemsSource = bl.Product.RequestList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.category));
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl.Product.RequestList();
            else
            {
                BO.category sortBy = (BO.category)CategorySelector.SelectedItem;
                ProductsListView.ItemsSource = bl.Product.RequestListByCond(i => i.Category == sortBy);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            new Product().ShowDialog();
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl.Product.RequestList();
            else
                ProductsListView.ItemsSource = bl.Product.RequestListByCond(i => i.Category == (BO.category)CategorySelector.SelectedItem);
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((ProductForList)ProductsListView.SelectedItem).ID;
            new Product(id).ShowDialog();
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl.Product.RequestList();
            else
                ProductsListView.ItemsSource = bl.Product.RequestListByCond(i => i.Category == (BO.category)CategorySelector.SelectedItem);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CategorySelector.SelectedItem = null;
            ProductsListView.ItemsSource = bl.Product.RequestList();
        }
    }
}
