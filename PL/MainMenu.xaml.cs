using PL.Manager;
using PL.OrderProcess;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        //Frame CatalogFrame = (Frame)MainWindow.FindName("CatalogFrame");
        public MainMenu()
        {
            InitializeComponent();
        }

        
        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            ManagerLogin manager=new();
            manager.Show();
        }

        private void OrderTrack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            //Catalouge catalouge=new Catalouge(bl);
            //catalouge.Show();
            if (MainWindow.mainFrame.CanGoBack)
            {
                MainWindow.mainFrame.RemoveBackEntry();
            }
            MainWindow.mainFrame.Navigate(new Uri("Catalog.xaml", UriKind.Relative));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
