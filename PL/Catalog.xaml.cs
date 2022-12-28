﻿using System;
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
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        IEnumerable<BO.ProductForList> products;
        public Catalog(BlApi.IBl? bl1)
        {
            bl = bl1;
            InitializeComponent();
            ProductsScrollView.DataContext = bl.Product.RequestList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.category));
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
    }
}
    

