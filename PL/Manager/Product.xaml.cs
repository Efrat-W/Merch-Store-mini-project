using BlApi;
using BlImplementation;
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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        private IBl bl = new Bl();
        public Product(IBl blOrigin, object originSender, ProductForList pForList)
        {
            InitializeComponent();
            bl = blOrigin;
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Update";

            BO.Product p = bl.Product.RequestByIdManager(pForList.ID);
            IdTB.Text = p.ID.ToString();
            NameTB.Text = p.Name;
            PriceTB.Text = p.Price.ToString();
            InStockTB.Text = p.InStock.ToString();
            CategoryCB.SelectedItem = p.Category;
        }
        public Product(IBl blOrigin, object originSender)
        {
            InitializeComponent();
            bl = blOrigin;
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Add";
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            BO.Product prod = new()
            {
                ID = int.Parse(IdTB.Text),
                Name = NameTB.Text,
                Price = double.Parse(PriceTB.Text),
                InStock = int.Parse(InStockTB.Text),
                Category = (BO.category)CategoryCB.SelectedItem
            };

            if (CommandBtn.Content == "Add")
            {
                bl.Product.Add(prod);
            }
            else
            {
                bl.Product.Update(prod);
            }
            Close();
        }
    }
}
