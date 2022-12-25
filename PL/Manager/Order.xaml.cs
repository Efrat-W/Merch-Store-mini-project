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
        public Order(int id)
        {
            InitializeComponent();
            //CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Update";

            BO.Order Ord = bl.Order.RequestById(id);
            IdTB.Text = Ord.Id.ToString();
            CustomerNameTB.Text = Ord.CustomerName;
            CustomerEmailTB.Text = Ord.CustomerEmail;
            CustomerAdressTB.Text = Ord.CustomerAddress;
            TotalPriceTB.Text = Ord.TotalPrice.ToString();
            
            //CategoryCB.SelectedItem = p.Category;
        }

        
    }
}
