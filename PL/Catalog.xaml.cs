using BO;
using DO;
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    private IEnumerable<BO.ProductItem> Products
    {
        get { return (IEnumerable<BO.ProductItem>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<BO.ProductItem>), typeof(Catalog));

    private Array categories
    {
        get { return (Array)GetValue(categoriesProperty); }
        set { SetValue(categoriesProperty, value); }
    }
    

    // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty categoriesProperty =
        DependencyProperty.Register("categories", typeof(Array), typeof(Catalog));

    private IEnumerable<IGrouping<BO.category?, ProductItem>> ProductsGroupedByCategory;
    private string groupName = "Category";
    PropertyGroupDescription propertyGroupDescription;
    private ICollectionView CollectionViewProductItemList { set; get; }

    public Catalog(BO.Cart cart1)
    {
        categories = Enum.GetValues(typeof(BO.category));
        cart = cart1;
        category = null;
        //grouping stuff
        ProductsGroupedByCategory = from ListProd in bl.Product.RequestList()
                                        let prod = new BO.ProductItem()
                                        {
                                            ID = ListProd.ID,
                                            Name = ListProd.Name,
                                            Description = ListProd.Description,
                                            Image = ListProd.Image,
                                            Price = ListProd.Price,
                                            Category = ListProd.Category,
                                            InStock = true,
                                            Amount = 0
                                        }
                                        orderby prod.Price
                                        group prod by prod.Category into CategoryGroup
                                        select CategoryGroup;
        Products = new List<ProductItem>();
        foreach (var categoryGroup in ProductsGroupedByCategory)
        {
            foreach (var item in categoryGroup)
            {
                Products = Products.Append(item);
            }
        }

        CollectionViewProductItemList = CollectionViewSource.GetDefaultView(Products);

        propertyGroupDescription = new PropertyGroupDescription(groupName);
        CollectionViewProductItemList.GroupDescriptions.Add(propertyGroupDescription);

        InitializeComponent();
    }
    /// <summary>
    /// constructor with category parameter
    /// </summary>
    /// <param name="cat"></param>
    /// <param name="cart1"></param>
    public Catalog(BO.category cat, BO.Cart cart1)
    {
        categories = Enum.GetValues(typeof(BO.category));
        cart = cart1;
        category = cat;

        ProductsGroupedByCategory = from ListProd in bl.Product.RequestList()
                                    let prod = new BO.ProductItem()
                                    {
                                        ID = ListProd.ID,
                                        Name = ListProd.Name,
                                        Description = ListProd.Description,
                                        Image = ListProd.Image,
                                        Price = ListProd.Price,
                                        Category = ListProd.Category,
                                        InStock = true,
                                        Amount = 0
                                    }
                                    group prod by prod.Category into CategoryGroup
                                    select CategoryGroup;
        Products = new List<ProductItem>();
        foreach (var categoryGroup in ProductsGroupedByCategory)
        {
            if (categoryGroup.Key == cat)
            {
                foreach (var item in categoryGroup)
                {
                    Products = Products.Append(item);
                }
            }
        }
        CollectionViewProductItemList = CollectionViewSource.GetDefaultView(Products);

        propertyGroupDescription = new PropertyGroupDescription(groupName);
        CollectionViewProductItemList.GroupDescriptions.Add(propertyGroupDescription);
        InitializeComponent();
    }
    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (category != null) {
            Products = new List<ProductItem>();
            foreach (var categoryGroup in ProductsGroupedByCategory)
            {
                if (categoryGroup.Key == category)
                {
                    foreach (var item in categoryGroup)
                    {
                        Products = Products.Append(item);
                    }
                }
            }
        }
    }
    /// <summary>
    /// clear selection by category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        category = null;
        Products = from ListProd in bl!.Product.RequestList()
                   select new BO.ProductItem()
                   {
                       ID = ListProd.ID,
                       Name = ListProd.Name,
                       Description = ListProd.Description,
                       Image = ListProd.Image,
                       Price = ListProd.Price,
                       Category = ListProd.Category,
                       InStock = true,
                       Amount = 0
                   };
        CollectionViewProductItemList = CollectionViewSource.GetDefaultView(Products);

        propertyGroupDescription = new PropertyGroupDescription(groupName);
        CollectionViewProductItemList.GroupDescriptions.Add(propertyGroupDescription);
        CollectionViewProductItemList.GroupDescriptions.Clear();
    }
    /// <summary>
    /// open the product window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseDoubleClick(object sender, MouseButtonEventArgs e) =>
        MainWindow.mainFrame.Navigate(new ViewProduct(((ProductItem)((ListView)sender).SelectedItem).ID, cart));
}



