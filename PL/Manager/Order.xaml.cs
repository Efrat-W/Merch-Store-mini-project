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
using System.Windows.Shapes;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();


        public BO.Order Ord
        {
            get { return (BO.Order)GetValue(OrdProperty); }
            set { SetValue(OrdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ord.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdProperty =
            DependencyProperty.Register("Ord", typeof(BO.Order), typeof(Order));


        public Array categories
        {
            get { return (Array)GetValue(categoriesProperty); }
            set { SetValue(categoriesProperty, value); }
        }


        // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty categoriesProperty =
            DependencyProperty.Register("categories", typeof(Array), typeof(Catalog));


        public Order(int id)
        {

            Ord = bl.Order.RequestById(id);
            InitializeComponent();
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        { 
            if (ShipCB.IsChecked == true) 
                bl?.Order.UpdateShipment(Ord.Id);
            if (DeliveryCB.IsChecked == true)
                try
                {
                    bl?.Order.UpdateDelivery(Ord.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot update delivery before commencing shipment.");
                }
            this.Close();
        }

    }
}
