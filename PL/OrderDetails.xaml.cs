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
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public BO.Order order
        {
            get { return (BO.Order)GetValue(orderProperty); }
            set { SetValue(orderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for order.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty orderProperty =
            DependencyProperty.Register("order", typeof(BO.Order), typeof(OrderDetails));


        public OrderDetails(int id)
        {
            InitializeComponent();
            try
            {
                order = bl.Order.RequestById(id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}