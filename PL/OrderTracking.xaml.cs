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

namespace PL;

/// <summary>
/// Interaction logic for OrderTracking.xaml
/// </summary>
public partial class OrderTracking : Page
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    private BO.OrderTracking track
    {
        get { return (BO.OrderTracking)GetValue(trackProperty); }
        set { SetValue(trackProperty, value); }
    }

    // Using a DependencyProperty as the backing store for track.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty trackProperty =
        DependencyProperty.Register("track", typeof(BO.OrderTracking), typeof(OrderTracking));
    
    int id;

    private bool trackVisibility
    {
        get { return (bool)GetValue(trackVisibilityProperty); }
        set { SetValue(trackVisibilityProperty, value); }
    }
    // Using a DependencyProperty as the backing store for trackVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty trackVisibilityProperty =
        DependencyProperty.Register("trackVisibility", typeof(bool), typeof(OrderTracking));

    private string text
    {
        get { return (string)GetValue(textProperty); }
        set { SetValue(textProperty, value); }
    }
    // Using a DependencyProperty as the backing store for text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty textProperty =
        DependencyProperty.Register("text", typeof(string), typeof(OrderTracking));

    public OrderTracking()
    {
        trackVisibility = false;
        InitializeComponent();
    }
    /// <summary>
    /// show order tracking details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TrackBtn_Click(object sender, RoutedEventArgs e)
    {
        int.TryParse(text, out id);
        try
        {
            track = bl!.Order.Track(id);
            trackVisibility = true;
        }
        catch(Exception ex)
        {
            MessageBox.Show($"Oops! No order with the id {id} could be found.");
        }
    }
    /// <summary>
    /// open order details page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.mainFrame.Navigate(new OrderDetails(id));
    }
}
