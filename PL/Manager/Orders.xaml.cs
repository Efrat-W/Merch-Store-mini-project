using BO;
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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public Orders(BlApi.IBl? bl1)
        {
            bl = bl1;
            InitializeComponent();

            var orderGroupsByStatus = from ord in bl!.Order.RequestOrders()
                                        group ord by ord.Status into statusGroup
                                        select statusGroup;
            OrdersListView.ItemsSource = orderGroupsByStatus;
        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersListView.SelectedItem == null) return;
            int id = ((OrderForList)OrdersListView.SelectedItem).ID;
            new Order(id).ShowDialog();
        }
    }
}
