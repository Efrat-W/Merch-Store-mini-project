using BO;
using Simulator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for SimulationWindow.xaml
/// </summary>
public partial class SimulationWindow : Window
{
    public string TimerText
    {
        get { return (string)GetValue(TimerTextProperty); }
        set { SetValue(TimerTextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TimerText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimerTextProperty =
        DependencyProperty.Register("TimerText", typeof(string), typeof(SimulationWindow));



    public int progress
    {
        get { return (int)GetValue(progressProperty); }
        set { SetValue(progressProperty, value); }
    }

    // Using a DependencyProperty as the backing store for progress.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty progressProperty =
        DependencyProperty.Register("progress", typeof(int), typeof(SimulationWindow));



    private Stopwatch stopWatch;
    private bool isTimerRun;
    BackgroundWorker timerworker;
    public SimulationWindow()
    {
        InitializeComponent();
        stopWatch = new Stopwatch();
        stopWatch.Start();
        timerworker = new BackgroundWorker();
        timerworker.DoWork += Worker_DoWork;
        timerworker.ProgressChanged += Worker_ProgressChanged;
        timerworker.WorkerReportsProgress = true;
        isTimerRun = true;
        timerworker.RunWorkerAsync();

    }
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (e.UserState is Tuple)
        {
            idLb.Content = ((Tuple<int, Order>)e.UserState).Item2.Id;
            statusLb.Content = ((Tuple<int, Order>)e.UserState).Item2.Status;
            updateToLb.Content = ((Tuple<int, Order>)e.UserState).Item2.Status == BO.orderStatus.Approved ? "Shipped" : "Delivered";
            estimatedFinishLb.Content = $"{((Tuple<int, Order>)e.UserState).Item1} seconds.";
        }

        //progress = e.ProgressPercentage;
        string timerText = stopWatch.Elapsed.ToString();
        TimerText = timerText.Substring(0, 8);
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        simulator.Init();
        //simulator.Report += Worker_ProgressChanged;
        while (isTimerRun)
        {
            timerworker.ReportProgress(1);
            Thread.Sleep(1000);
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        base.OnClosing(e);
    }
    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        if (isTimerRun)
        {
            stopWatch.Stop();
            isTimerRun = false;
            Close();
        }
    }

    //public void ReportLog(Object sender, EventArgs e)
    //{
        
    //}
}
