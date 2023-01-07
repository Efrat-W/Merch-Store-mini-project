using BO;
using DO;
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

namespace PL;

/// <summary>
/// Interaction logic for Catalog.xaml
/// </summary>
public partial class Catalog : Page
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    private static BO.Cart cart;
    private BO.category? category
    {
        get { return (BO.category?)GetValue(categoryProperty); }
        set { SetValue(categoryProperty, value); }
    }

    // Using a DependencyProperty as the backing store for category.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty categoryProperty =
        DependencyProperty.Register("category", typeof(BO.category?), typeof(Catalog));

    public static readonly DependencyProperty ProductsDependency =
        DependencyProperty.Register(nameof(Products), typeof(ObservableCollection<ProductForList?>), typeof(Catalog));
    public ObservableCollection<ProductForList?> Products
    {
        get => (ObservableCollection<ProductForList?>)GetValue(ProductsDependency);
        private set => SetValue(ProductsDependency, value);
    }


    public Array categories
    {
        get { return (Array)GetValue(categoriesProperty); }
        set { SetValue(categoriesProperty, value); }
    }

    // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty categoriesProperty =
        DependencyProperty.Register("categories", typeof(Array), typeof(Catalog));

    public Catalog(BO.Cart cart1)
    {
        cart = cart1;
        category = null;
        InitializeComponent();
        Products = new(bl.Product.RequestList());
        categories = Enum.GetValues(typeof(BO.category));
    }
    public Catalog(BO.category cat, BO.Cart cart1)
    {
        cart=cart1;
        category = cat;
        InitializeComponent();
        categories = Enum.GetValues(typeof(BO.category));
        Products = new(bl.Product.RequestListByCond(i => i.Category == cat));
    }
    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (CategorySelector.SelectedItem == null)
            Products = new(bl.Product.RequestList());
        else
        {
            BO.category sortBy = (BO.category)CategorySelector.SelectedItem;
            Products = new(bl.Product.RequestListByCond(i => i.Category == sortBy));
        }
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        category = null;
        Products = new(bl.Product.RequestList());
    }

    private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        int id = ((ProductForList)ProductsScrollView.SelectedItem).ID;
        MainWindow.mainFrame.Navigate(new ViewProduct(id, cart));
    }
}



