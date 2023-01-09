
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

        


        public string CommandBtnDP
        {
            get { return (string)GetValue(CommandBtnDPProperty); }
            set { SetValue(CommandBtnDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandBtnDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandBtnDPProperty =
            DependencyProperty.Register("CommandBtnDP", typeof(string), typeof(Product));


        public Array categories
        {
            get { return (Array)GetValue(categoriesProperty); }
            set { SetValue(categoriesProperty, value); }
        }


        // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty categoriesProperty =
            DependencyProperty.Register("categories", typeof(Array), typeof(Catalog));



        public Product(int id)
        {
            categories = Enum.GetValues(typeof(BO.category));
            product = bl.Product.RequestByIdManager(id);
            CommandBtnDP = "Update";
            InitializeComponent();
        }
        public Product()
        {
            categories = Enum.GetValues(typeof(BO.category));
            product = new BO.Product();
            CommandBtnDP = "Add";
            InitializeComponent();
        }

        public BO.Product? product
        {
            get { return (BO.Product)GetValue(productProperty); }
            set { SetValue(productProperty, value); }
        }

        // Using a DependencyProperty as the backing store for product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty productProperty =
            DependencyProperty.Register("product", typeof(BO.Product), typeof(Product));

        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommandBtnDP == "Add")
                {
                    bl?.Product.Add(product);
                    MessageBox.Show($"{product.Name} was added successfully.");
                }
                else // "Update"
                {
                    bl?.Product.Update(product);
                    MessageBox.Show($"{product.Name} was updated successfully.");
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more of the input fields were filled in incorrectly.\nHover over the fields in order to see the according restrictions.");
            }

        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl?.Product.Delete(product.ID);
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
            if ((!(IdTB.Text.Length == 0) && int.Parse(IdTB.Text) > int.MaxValue / 10) || (!(InStockTB.Text.Length == 0) && int.Parse(InStockTB.Text) > int.MaxValue / 10))
            {
                e.Handled = true;
                return;
            }
            if (!(InStockTB.Text.Length == 0) && int.Parse(InStockTB.Text) > int.MaxValue / 10)
            {
                e.Handled = true;
                return;
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

    }
}
