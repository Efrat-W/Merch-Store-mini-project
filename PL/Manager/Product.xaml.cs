
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public Product(int id)
        {
            InitializeComponent();
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Update";

            BO.Product p = bl.Product.RequestByIdManager(id);
            IdTB.Text = p.ID.ToString();
            IdTB.IsReadOnly = true;
            NameTB.Text = p.Name;
            PriceTB.Text = p.Price.ToString();
            InStockTB.Text = p.InStock.ToString();
            CategoryCB.SelectedItem = p.Category;
            ImageTB.Text = p.Image;
            DescriptionTB.Text = p.Description;
        }
        public Product()
        {
            InitializeComponent();
            CategoryCB.ItemsSource = Enum.GetValues(typeof(BO.category));
            CommandBtn.Content = "Add";
        }

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            //check if the attributes where filled
            if (string.IsNullOrWhiteSpace(NameTB.Text) || string.IsNullOrWhiteSpace(IdTB.Text) || string.IsNullOrWhiteSpace(PriceTB.Text) || string.IsNullOrWhiteSpace(InStockTB.Text))
            {
                MessageBox.Show("Make sure you fill out the entire form.");
                return;
            }

            try
            {
                BO.Product prod = new()
                {
                    ID = int.Parse(IdTB.Text),
                    Name = NameTB.Text,
                    Price = double.Parse(PriceTB.Text),
                    InStock = int.Parse(InStockTB.Text),
                    Category = (BO.category)CategoryCB.SelectedItem,
                    Image=ImageTB.Text,
                    Description = DescriptionTB.Text,
                };

                if (CommandBtn.Content == "Add")
                { 
                    if (IdTB.Text.Length < 6)
                    {
                        InvalidIdLb.Content = "Fill in 6 digits.";
                        InvalidIdLb.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                        InvalidIdLb.Visibility = Visibility.Hidden;
                    bl?.Product.Add(prod); 
                }
                else // "Update"
                    bl?.Product.Update(prod);
                Close();
            }
            catch
            {
                MessageBox.Show("An unexpected error has occured.\nMake sure you fill it out correctly.");
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl?.Product.Delete(int.Parse(IdTB.Text));
                Close();
            }
            catch
            {
                MessageBox.Show("An unexpected error has occured.\nMake sure you fill it out correctly.");
            }
        }

        private void PreviewTextInputDigits(object sender, TextCompositionEventArgs e)
        {
            //restrict size of input
            if (!(IdTB.Text.Length == 0) && int.Parse(IdTB.Text) > int.MaxValue / 10)
            {
                e.Handled = true;
                InvalidIdLb.Content = "Input too big.";
                InvalidIdLb.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                InvalidIdLb.Visibility = Visibility.Hidden;
                InvalidIdLb.Content = "Fill in ID.";
            }
            if (!(InStockTB.Text.Length == 0) && int.Parse(InStockTB.Text) > int.MaxValue / 10)
            {
                e.Handled = true;
                InvalidInStockLb.Content = "Input too big.";
                InvalidInStockLb.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                InvalidInStockLb.Visibility = Visibility.Hidden;
                InvalidInStockLb.Content = "Fill in amount.";
            }
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PreviewTextInputDecimal(object sender, TextCompositionEventArgs e)
        {
            //restrict size of input
            if (PriceTB.Text.Length > 8)
                PriceTB.Text = PriceTB.Text.Substring(0, 8);

            //append '0.' whenever user inputs a dot at the start
            if (e.Text.Contains('.') && string.IsNullOrWhiteSpace(PriceTB.Text))
            {
                PriceTB.Text = "0.";
                e.Handled = true;
                return;
            }

            //regex restriction of invalid chars in event, and unnecessary '.'
            Regex regex = new("[^0-9.]+");
            e.Handled = (e.Text.Contains('.') && PriceTB.Text.Contains('.')) ? true : e.Handled = regex.IsMatch(e.Text);

            //restriction of 2 digits after the dot.
            int index = PriceTB.Text.IndexOf('.');
            if (index != -1 && PriceTB.Text.Substring(index).Length > 2)
            {
                e.Handled = true;
                string s = PriceTB.Text.Substring(0, index);
                s += ".";
                s += PriceTB.Text.Substring(index + 1, 2);
                PriceTB.Text = s;
            }
        }

        //textbox focus methods
        private void NameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTB.Text))
                InvalidNameLb.Visibility = Visibility.Visible;
        }

        private void NameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            InvalidNameLb.Visibility = Visibility.Hidden;
        }

        private void IdTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IdTB.Text))
                InvalidIdLb.Visibility = Visibility.Visible;
        }
        private void IdTB_GotFocus(object sender, RoutedEventArgs e)
        {
            InvalidIdLb.Visibility = Visibility.Hidden;
        }

        private void PriceTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PriceTB.Text))
                InvalidPriceLb.Visibility = Visibility.Visible;
        }

        private void PriceTB_GotFocus(object sender, RoutedEventArgs e)
        {
            InvalidPriceLb.Visibility = Visibility.Hidden;
        }

        private void InStockTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InStockTB.Text))
                InvalidInStockLb.Visibility = Visibility.Visible;
        }

        private void InStockTB_GotFocus(object sender, RoutedEventArgs e)
        {
            InvalidInStockLb.Visibility = Visibility.Hidden;
        }

    }
}
