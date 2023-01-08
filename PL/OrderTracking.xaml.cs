﻿using System;
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
    public BO.OrderTracking track
    {
        get { return (BO.OrderTracking)GetValue(trackProperty); }
        set { SetValue(trackProperty, value); }
    }

    // Using a DependencyProperty as the backing store for track.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty trackProperty =
        DependencyProperty.Register("track", typeof(BO.OrderTracking), typeof(OrderTracking));
    int id;


    public OrderTracking()
    {
        InitializeComponent();
    }

    private void TrackBtn_Click(object sender, RoutedEventArgs e)
    {
        id = int.Parse(IdTB.Text);
        try
        {
            track = bl!.Order.Track(id);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        SignIn.Visibility = Visibility.Collapsed;
        Tracking.Visibility = Visibility.Visible;
    }

    private void DetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.mainFrame.Navigate(new OrderDetails(id));
    }
}
