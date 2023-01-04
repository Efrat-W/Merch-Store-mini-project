using BO;
using DO;
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;

    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Page
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        private static BO.Cart cart = new();
        int ID;
        public ViewProduct(int id, BO.Cart cart1)
        {
            cart=cart1; 
            ID = id;
            InitializeComponent();
            MainGrid.DataContext = bl.Product.RequestByIdManager(id);
        }
    
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            bl.Cart.AddProduct(cart, ID);
            MessageBox.Show($"New item was added successfully to your cart!");
        }
    }

