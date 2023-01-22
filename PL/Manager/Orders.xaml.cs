using BO;
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
using System.Windows.Shapes;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        private ObservableCollection<BO.OrderForList> OrdersDP
        {
            get { return (ObservableCollection<BO.OrderForList>)GetValue(OrdersProperty); }
            set { SetValue(OrdersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdersProperty =
            DependencyProperty.Register("Products", typeof(ObservableCollection<BO.OrderForList>), typeof(Orders));

        public Orders(BlApi.IBl? bl1)
        {
            bl = bl1;
            var orderGroupsByStatus = from ord in bl!.Order.RequestOrders()
                                      orderby ord.ID
                                      group ord by ord.Status into statusGroup
                                      select statusGroup;
            OrdersDP = new ObservableCollection<BO.OrderForList>();
            foreach (var categoryGroup in orderGroupsByStatus)
            {
                foreach (var item in categoryGroup)
                {
                    OrdersDP.Add(item);
                }
            }
            InitializeComponent();
        }
        /// <summary>
        /// open the selected order window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            var senderLS = (ListView)sender;
            if (senderLS.SelectedItem != null)
                new Order(((OrderForList)(senderLS.SelectedItem)).ID).ShowDialog();
            new Orders(bl).Show();
            Close();
        }
    }
}
