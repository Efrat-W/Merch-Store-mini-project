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

namespace PL
{
    /// <summary>
    /// Interaction logic for OrderTracking.xaml
    /// </summary>
    public partial class OrderTracking : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        BO.OrderTracking track;
        public OrderTracking()
        {
            InitializeComponent();
            
        }

        private void TrackBtn_Click(object sender, RoutedEventArgs e)
        {
            SignIn.Visibility = Visibility.Collapsed;
            Tracking.Visibility = Visibility.Visible;
            track = bl.Order.Track(int.Parse(IdTB.Text));
            Tracking.DataContext=track;
            DatesAndStuff.DataContext = track.orderProgress;
            if (track.Status == BO.orderStatus.Shipped || track.Status == BO.orderStatus.Delivered)
            {
                ShipBtn.Background = Brushes.Black;
                ShipBtn.Content = "✔️";
                ShipDate.Content = track.orderProgress[1];
                if (track.Status == BO.orderStatus.Delivered)
                {
                    DeliverBtn.Background = Brushes.Black;
                    DeliverBtn.Content = "✔️";
                    DeliveryDate.Content = track.orderProgress[2];
                }
                else
                    DeliveryDate.Content = "Not yet";
            }
            else
            {
                ShipDate.Content = "Not yet";
                DeliveryDate.Content = "Not yet";
            }
            }
    }
}
