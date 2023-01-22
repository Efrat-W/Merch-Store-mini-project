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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private static BO.Cart cart = new();
        public HomePage( BO.Cart cart1)
        {
            cart = cart1;
            InitializeComponent();
        }
        /// <summary>
        /// open the catalog window ordered by category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCollBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.mainFrame.CanGoBack)
            {
                MainWindow.mainFrame.RemoveBackEntry();
            }
            BO.category category = BO.category.Notebooks;
            MainWindow.mainFrame.Navigate(new Catalog(category, cart));
        
        }
    }
    
}
