using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        //ICollectionView ProductsCollectionView;
        public ObservableCollection<BO.ProductForList> ProductsDP
        {
            get
            {
                return (ObservableCollection<BO.ProductForList>)GetValue(ProductsDPProperty);
            }
            set
            {
                SetValue(ProductsDPProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductsDPProperty =
            DependencyProperty.Register("ProductsDP", typeof(ObservableCollection<BO.ProductForList>), typeof(Products));


        public Array Categories
        {
            get { return (Array)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(Array), typeof(Products));



        public BO.category? selectedCategory
        {
            get { return (BO.category?)GetValue(selectedCategoryProperty); }
            set { SetValue(selectedCategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for selectedCategory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selectedCategoryProperty =
            DependencyProperty.Register("selectedCategory", typeof(BO.category?), typeof(Products));


        public Products(BlApi.IBl? bl1)
        {
            bl = bl1;
            ProductsDP = new ObservableCollection<ProductForList>(bl!.Product.RequestList());
            Categories = Enum.GetValues(typeof(BO.category));
            //ProductsCollectionView = CollectionViewSource.GetDefaultView(ProductsDP);
            InitializeComponent();
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductsDP.Clear();
            if (selectedCategory != null)
            {
                List<BO.ProductForList> list = bl!.Product.RequestListByCond(i => i.Category == selectedCategory).ToList();
                foreach (var item in list)
                    ProductsDP.Add(item);
            }
            else
            {
                List<BO.ProductForList> list = bl!.Product.RequestList().ToList();
                foreach (var item in list)
                    ProductsDP.Add(item);
            }
            //ProductsCollectionView.Refresh();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            new Product().ShowDialog();
            if (selectedCategory == null)
                ProductsDP = new ObservableCollection<ProductForList>(bl!.Product.RequestList());
            else
                ProductsDP = new ObservableCollection<ProductForList>(bl!.Product.RequestListByCond(i => i.Category == selectedCategory));
            //ProductsCollectionView.Refresh();
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var senderLS = (ListView)sender;
            if (senderLS.SelectedItem != null)
                new Product(((ProductForList)(senderLS.SelectedItem)).ID).ShowDialog();
            if (selectedCategory == null)
                ProductsDP = new ObservableCollection<ProductForList>(bl!.Product.RequestList());
            else
                ProductsDP = new ObservableCollection<ProductForList>(bl!.Product.RequestListByCond(i => i.Category == selectedCategory));
            //ProductsCollectionView.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedCategory = null;
        }
    }


}
