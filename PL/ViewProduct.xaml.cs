﻿using BO;
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
    private BO.Cart cart
    {
        get { return (BO.Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cart", typeof(BO.Cart), typeof(ViewProduct));

    private BO.ProductItem Product
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
    /// <summary>
    /// add the product to cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddToCart_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Product.Amount == 0)
                cart = bl!.Cart.AddProduct(cart, Product.ID);
            Product = bl!.Product.RequestByIdCustomer(Product.ID, cart);
        }
        catch(Exception ex)
        {
            MessageBox.Show("Uh oh! \nAn error has occured");
        }

    }
    /// <summary>
    /// increase product amount on cart by 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        try 
        { 
            cart = bl!.Cart.UpdateProductAmount(cart, Product.ID , 1);
            Product = bl.Product.RequestByIdCustomer(Product.ID, cart);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Uh oh! \nThere's not enough {Product.Name} in stock.");
        }
    }
    /// <summary>
    /// decrease product amount on cart by 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            cart = bl!.Cart.UpdateProductAmount(cart, Product.ID, -1);
            Product = bl.Product.RequestByIdCustomer(Product.ID, cart);
        }   
        catch(Exception ex)
        {
            MessageBox.Show($"Uh oh! \nWe cannot decrease the amount of {Product.Name} in your cart.");
        }
    }
}

