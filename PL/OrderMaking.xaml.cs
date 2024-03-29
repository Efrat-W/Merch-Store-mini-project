﻿using BlImplementation;
using System;
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

namespace PL.OrderProcess
{
    /// <summary>
    /// Interaction logic for OrderMaking.xaml
    /// </summary>
    public partial class OrderMaking : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        private BO.Cart cart
        {
            get { return (BO.Cart)GetValue(cartProperty); }
            set { SetValue(cartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cartProperty =
            DependencyProperty.Register("cart", typeof(BO.Cart), typeof(OrderMaking));


        public OrderMaking(BO.Cart cart1)
        {
            InitializeComponent();
            cart = cart1;
        }
        /// <summary>
        /// make the order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BO.Order order = new();
            try
            {
                order = bl!.Cart.Approve(cart);
            }
            catch (InvalidArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (order.Id != 0)
            {
                MessageBox.Show($@" HOORAY! 
Your order has been placed successfully and will soon be on its way. We can't wait!
Your order id is: {order.Id}. A verification mail is already in your inbox.");
                //empty cart
                bl!.Cart.Empty(cart);
            }
        }
       
    }
}
