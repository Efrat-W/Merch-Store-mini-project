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
using System.Windows.Shapes;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        private BO.Order Ord
        {
            get { return (BO.Order)GetValue(OrdProperty); }
            set { SetValue(OrdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ord.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdProperty =
            DependencyProperty.Register("Ord", typeof(BO.Order), typeof(Order));


        private bool IsShipped
        {
            get { return (bool)GetValue(IsShippedProperty); }
            set { SetValue(IsShippedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isShipped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShippedProperty =
            DependencyProperty.Register("IsShipped", typeof(bool), typeof(Order));

        private bool IsDelivered
        {
            get { return (bool)GetValue(IsDeliveredProperty); }
            set { SetValue(IsDeliveredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isShipped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDeliveredProperty =
            DependencyProperty.Register("IsDelivered", typeof(bool), typeof(Order));

        public Order(int id)
        {
            Ord = bl.Order.RequestById(id);
            InitializeComponent();
        }
        /// <summary>
        /// if need, updates shipment/delivery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        { 
            if (IsShipped) 
                bl?.Order.UpdateShipment(Ord.Id);
            if (IsDelivered)
                try
                {
                    bl?.Order.UpdateDelivery(Ord.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot update delivery before commencing shipment.");
                }
            Close();
        }

    }
}
