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
        public Product(IBl blOrigin, int id)
        {
            InitializeComponent();
            bl = blOrigin;
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Update";

            BO.Product p = bl.Product.RequestByIdManager(id);
            IdTB.Text = p.ID.ToString();
            NameTB.Text = p.Name;
            PriceTB.Text = p.Price.ToString();
            InStockTB.Text = p.InStock.ToString();
            CategoryCB.SelectedItem = p.Category;
        }
        public Product(IBl blOrigin)
        {
            InitializeComponent();
            bl = blOrigin;
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Add";
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            //check fields correctness
            if (int.Parse(IdTB.Text) <= 99999 || int.Parse(IdTB.Text) > 999999)
            {
                IdTB.Text = "X Invalid price.";
                IdTB.Foreground = Brushes.Red;
            }
            double.TryParse(PriceTB.Text, out double price);
            if (price < 0)
            {
                PriceTB.Text = "X Invalid price.";
                PriceTB.Foreground = Brushes.Red;
            }
            if (int.Parse(InStockTB.Text) < 0)
                Console.WriteLine();
                


            try
            {
                BO.Product prod = new()
                {
                    ID = int.Parse(IdTB.Text),
                    Name = NameTB.Text,
                    Price = price,
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
            catch
            {
                MessageBox.Show("One or more fields where filled in incorrectly.");
            }
        }

      
    }
}
