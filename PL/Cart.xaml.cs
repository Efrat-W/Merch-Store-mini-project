using BO;
using PL.OrderProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace PL;

/// <summary>
/// Interaction logic for Cart.xaml
/// </summary>
public partial class Cart : Page
{
    BlApi.IBl? bl = BlApi.Factory.Get();

    public BO.Cart cart
    {
        get { return (BO.Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
       DependencyProperty.Register("cart", typeof(BO.Cart), typeof(Cart));
    public BO.OrderItem item
    {
        get { return (BO.OrderItem)GetValue(itemProperty); }
        set { SetValue(itemProperty, value); }
    }

    // Using a DependencyProperty as the backing store for item.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty itemProperty =
        DependencyProperty.Register("item", typeof(BO.OrderItem), typeof(Cart));


    public Cart(BO.Cart cart1)
    {
        cart = cart1;
        InitializeComponent();
    }
    private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        int id = ((OrderItem)ProductsScrollView.SelectedItem).ProductId;
        MainWindow.mainFrame.Navigate(new ViewProduct(id,cart));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (MainWindow.mainFrame.CanGoBack)
            MainWindow.mainFrame.RemoveBackEntry();
        
        MainWindow.mainFrame.Navigate(new OrderMaking(cart));
       
    }
    private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        item = (BO.OrderItem)((Button)sender).DataContext;
        try
        {
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, 1);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        item = (BO.OrderItem)((Button)sender).DataContext;
        try
        {
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, -1);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void RemoveBtn_Click(object sender, RoutedEventArgs e)
    {
        item = (BO.OrderItem)((Button)sender).DataContext;
        try
        {
            cart = bl.Cart.UpdateProductAmount(cart, item.ProductId, 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
