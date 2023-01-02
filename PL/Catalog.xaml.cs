using BO;
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        //ObservableCollection<BO.ProductForList> products
        //{
        //    get { return productsProperty; }
        //    set { SetValue(productsProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for products.
        //public static readonly DependencyProperty productsProperty =
        //DependencyProperty.Register("products", typeof(bool), typeof(Catalog), new PropertyMetadata(true));

        public Catalog()
        {
            InitializeComponent();
            ProductsScrollView.DataContext = bl.Product.RequestList();
            //products = new(bl.Product.RequestList());
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.category));
        }
        public Catalog(BO.category category)
        {
            InitializeComponent();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.category));
            ProductsScrollView.DataContext = bl.Product.RequestListByCond(i => i.Category == category);
            CategorySelector.SelectedItem=category;
        }
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CategorySelector.SelectedItem == null)
                ProductsScrollView.DataContext = bl.Product.RequestList();
            else
            {
                BO.category sortBy = (BO.category)CategorySelector.SelectedItem;
                ProductsScrollView.DataContext = bl.Product.RequestListByCond(i => i.Category == sortBy);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CategorySelector.SelectedItem = null;
            ProductsScrollView.DataContext = bl.Product.RequestList();
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((ProductForList)ProductsScrollView.SelectedItem).ID;
            new ViewProduct(id).ShowDialog();
        }
    }
}
    

