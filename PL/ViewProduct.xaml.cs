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
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
public partial class ViewProduct : Page
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    public BO.Cart cart
    {
        get { return (BO.Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cart", typeof(BO.Cart), typeof(ViewProduct));

    public BO.ProductItem Product
    {
        get { return (BO.ProductItem)GetValue(ProductProperty); }
        set { SetValue(ProductProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductProperty =
        DependencyProperty.Register("Product", typeof(BO.ProductItem), typeof(ViewProduct));

    public ViewProduct(int id, BO.Cart cart1)
    {
            cart=cart1; 
            InitializeComponent();
            Product = bl.Product.RequestByIdCustomer(id,cart);
    }
    
    private void AddToCart_Click(object sender, RoutedEventArgs e)
    {
        cart=bl.Cart.AddProduct(cart, Product.ID);
        
    }
    private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        cart = bl.Cart.UpdateProductAmount(cart, Product.ID , 1);
       

    }
    private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        cart = bl.Cart.UpdateProductAmount(cart, Product.ID, -1);
       
    }
}

