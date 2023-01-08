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
        BlApi.IBl? bl = BlApi.Factory.Get();

        public IEnumerable<BO.ProductForList> ProductsDP
        {
            get { return (IEnumerable<BO.ProductForList>)GetValue(ProductsDPProperty); }
            set { SetValue(ProductsDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsDPProperty =
            DependencyProperty.Register("Products", typeof(IEnumerable<BO.ProductForList>), typeof(Products));

     
        public Array Categories
        {
            get { return (Array)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(Array), typeof(Products));

        public Products(BlApi.IBl? bl1)
        {
            bl = bl1;
            ProductsDP = bl!.Product.RequestList().Select(i => i);
            Categories = Enum.GetValues(typeof(BO.category));

            InitializeComponent();
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl!.Product.RequestList();
            else
            {
                BO.category sortBy = (BO.category)CategorySelector.SelectedItem;
                ProductsListView.ItemsSource = bl!.Product.RequestListByCond(i => i.Category == sortBy);
            }
            //if (CategorySelector.SelectedItem != null)
            //{
            //    ProductsDP.GroupBy(category => category.Name).Where(category => category == CategorySelector.SelectedItem).Select(i => i.Key);
            //}
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            new Product().ShowDialog();
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl!.Product.RequestList();
            else
                ProductsListView.ItemsSource = bl!.Product.RequestListByCond(i => i.Category == (BO.category)CategorySelector.SelectedItem);
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductsListView.SelectedItem == null) return;
            int id = ((ProductForList)ProductsListView.SelectedItem).ID;
            new Product(id).ShowDialog();
            if (CategorySelector.SelectedItem == null)
                ProductsListView.ItemsSource = bl!.Product.RequestList();
            else
                ProductsListView.ItemsSource = bl!.Product.RequestListByCond(i => i.Category == (BO.category)CategorySelector.SelectedItem);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CategorySelector.SelectedItem = null;
            ProductsListView.ItemsSource = bl!.Product.RequestList();
        }
    }


}
