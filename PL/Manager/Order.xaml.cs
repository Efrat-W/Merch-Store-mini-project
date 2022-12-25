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
            //CommandBtn.Content = "Update";

            //BO.Product p = bl.Product.RequestByIdManager(id);
            //IdTB.Text = p.ID.ToString();
            //IdTB.IsReadOnly = true;
            //NameTB.Text = p.Name;
            //PriceTB.Text = p.Price.ToString();
            //InStockTB.Text = p.InStock.ToString();
            //CategoryCB.SelectedItem = p.Category;
        }
    }
}
