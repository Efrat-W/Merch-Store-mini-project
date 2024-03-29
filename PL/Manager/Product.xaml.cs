﻿
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string CommandBtnDP
        {
            get { return (string)GetValue(CommandBtnDPProperty); }
            set { SetValue(CommandBtnDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandBtnDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandBtnDPProperty =
            DependencyProperty.Register("CommandBtnDP", typeof(string), typeof(Product));

        public Array categoriesDP
        {
            get { return (Array)GetValue(categoriesDPProperty); }
            set { SetValue(categoriesDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty categoriesDPProperty =
            DependencyProperty.Register("categoriesDP", typeof(Array), typeof(Catalog));
        private BO.Product? product
        {
            get { return (BO.Product)GetValue(productProperty); }
            set { SetValue(productProperty, value); }
        }

        // Using a DependencyProperty as the backing store for product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty productProperty =
            DependencyProperty.Register("product", typeof(BO.Product), typeof(Product));

        /// <summary>
        /// update window constructor
        /// </summary>
        /// <param name="id"></param>
        public Product(int id)
        {
            categoriesDP = Enum.GetValues(typeof(BO.category));
            product = bl.Product.RequestByIdManager(id);
            CommandBtnDP = "Update";
            InitializeComponent();
        }
        /// <summary>
        /// add window constructor
        /// </summary>
        public Product()
        {
            categoriesDP = Enum.GetValues(typeof(BO.category));
            product = new BO.Product();
            CommandBtnDP = "Add";
            InitializeComponent();
        }
        /// <summary>
        /// add /update the product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBtn_Click(object sender, RoutedEventArgs e)
        {
            //making sure the photo exists in the images folder and saves it properly
            if(product!.Image != null)
            {
                string ImageString=product.Image.Substring(product.Image.LastIndexOf("\\"));
                if (!File.Exists(Environment.CurrentDirectory[..^4] + @"\PL\Images\" + ImageString))
                    File.Copy(product.Image, Environment.CurrentDirectory[..^4] + @"\PL\Images\" + ImageString);
                product.Image = @"\Images"+ImageString;
            }
            try
            {
                if (CommandBtnDP == "Add")
                {
                    bl?.Product.Add(product);
                    MessageBox.Show($"{product.Name} was added successfully to the system.");
                }
                else // "Update"
                {
                    bl?.Product.Update(product);
                    MessageBox.Show($"{product.Name} was updated successfully to the system.");
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more of the input fields were filled in incorrectly.\nHover over the fields in order to see the according restrictions.");
            }

        }
        /// <summary>
        /// delete the product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl?.Product.Delete(product!.ID);
                Close();
            }
            catch
            {
                MessageBox.Show("An unexpected error has occured.\nMake sure you fill it out correctly.");
            }
        }
        /// <summary>
        /// open the file explorer for adding an image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                product!.Image=openFileDialog.FileName;
            }
        }
        /// <summary>
        /// preview key inputs if meeting the requirements of an integer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextInputDigits(object sender, TextCompositionEventArgs e)
        {
            var tb = ((TextBox)sender);
            //restrict size of input
            if ((!(tb.Text.Length == 0) && int.Parse(tb.Text) > int.MaxValue / 10))
            {
                e.Handled = true;
                return;
            }
            
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// preview key inputs if meeting the requirements of a decimal number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextInputDecimal(object sender, TextCompositionEventArgs e)
        {
            var tb = ((TextBox)sender);
            //restrict size of input
            if (tb.Text.Length > 8)
                tb.Text = tb.Text.Substring(0, 8);

            //append '0.' whenever user inputs a dot at the start
            if (e.Text.Contains('.') && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "0.";
                e.Handled = true;
                return;
            }

            //regex restriction of invalid chars in event, and unnecessary '.'
            Regex regex = new("[^0-9.]+");
            e.Handled = (e.Text.Contains('.') && tb.Text.Contains('.')) ? true : e.Handled = regex.IsMatch(e.Text);

            //restriction of 2 digits after the dot.
            int index = tb.Text.IndexOf('.');
            if (index != -1 && tb.Text.Substring(index).Length > 2)
            {
                e.Handled = true;
                string s = tb.Text.Substring(0, index);
                s += ".";
                s += tb.Text.Substring(index + 1, 2);
                tb.Text = s;
            }
        }

    }
}
