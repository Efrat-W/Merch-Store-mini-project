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
        BO.Order Ord;
        public Order(int id)
        {
            InitializeComponent();
            StatusCB.ItemsSource = Enum.GetValues(typeof(BO.orderStatus));
            CommandBtn.Content = "Update";

            Ord = bl.Order.RequestById(id);
            IdTB.Text = Ord.Id.ToString();
            CustomerNameTB.Text = Ord.CustomerName;
            CustomerEmailTB.Text = Ord.CustomerEmail;
            CustomerAdressTB.Text = Ord.CustomerAddress;
            TotalPriceTB.Text = Ord.TotalPrice.ToString();
            OrderDateTB.Text = Ord.OrderDate.ToString();
            if (Ord.ShipDate == null)
            {
                ShipCB.IsEnabled = true;
                ShipDateTB.IsEnabled = false;
                ShipDateLb.IsEnabled = false;
            }
            else
                ShipDateTB.Text = Ord.ShipDate.ToString();
            if (Ord.DeliveryDate == null)
            {
                DeliveryCB.IsEnabled = true;
                DeliveryDateTB.IsEnabled = false;
                DeliveryDateLb.IsEnabled = false;
            }
            else
                DeliveryDateTB.Text = Ord.DeliveryDate.ToString();
            StatusCB.SelectedItem = Ord.Status;
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        { 
            if(ShipCB.IsChecked==true)
                bl?.Order.UpdateShipment(Ord.Id);
            if(DeliveryCB.IsChecked==true)
                bl?.Order.UpdateDelivery(Ord.Id);
        }

    }
}
