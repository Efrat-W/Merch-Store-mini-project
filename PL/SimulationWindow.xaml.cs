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



    public int IdLbContent
    {
        get { return (int)GetValue(IdLbContentProperty); }
        set { SetValue(IdLbContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IdLbContentProperty =
        DependencyProperty.Register("IdLbContent", typeof(int), typeof(SimulationWindow));

    public string StatusContent
    {
        get { return (string)GetValue(StatusContentProperty); }
        set { SetValue(StatusContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusContentProperty =
        DependencyProperty.Register("StatusContent", typeof(string), typeof(SimulationWindow));

    public string StatusUpdatedContent
    {
        get { return (string)GetValue(StatusUpdatedContentProperty); }
        set { SetValue(StatusUpdatedContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusUpdatedContentProperty =
        DependencyProperty.Register("StatusUpdatedContent", typeof(string), typeof(SimulationWindow));



    public string estimatedFinishStatus
    {
        get { return (string)GetValue(estimatedFinishStatusProperty); }
        set { SetValue(estimatedFinishStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for estimatedFinishStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty estimatedFinishStatusProperty =
        DependencyProperty.Register("estimatedFinishStatus", typeof(string), typeof(SimulationWindow));



    public string ProgressStatus
    {
        get { return (string)GetValue(ProgressStatusProperty); }
        set { SetValue(ProgressStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProgressStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressStatusProperty =
        DependencyProperty.Register("ProgressStatus", typeof(string), typeof(SimulationWindow));



    public bool isRunning
    {
        get { return (bool)GetValue(isRunningProperty); }
        set { SetValue(isRunningProperty, value); }
    }

    // Using a DependencyProperty as the backing store for isRunning.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty isRunningProperty =
        DependencyProperty.Register("isRunning", typeof(bool), typeof(SimulationWindow), new PropertyMetadata(0));




    private Stopwatch stopWatch;
    private bool isTimerRun;
    BackgroundWorker timerworker;
    public SimulationWindow()
    {
        IdLbContent = 0;
        StatusContent = StatusUpdatedContent = "Loading...";
        InitializeComponent();
        Closing += (s, e) => e.Cancel = true;
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
        switch (e.ProgressPercentage)
        {
            case 0: //timer
                string timerText = stopWatch.Elapsed.ToString();
                TimerText = timerText.Substring(0, 8);
                break;
            case 1:
                IdLbContent = (e.UserState as Tuple<int, Order>).Item2.Id;
                StatusContent = (e.UserState as Tuple<int, Order>).Item2.Status.ToString();
                StatusUpdatedContent = (e.UserState as Tuple<int, Order>).Item2.Status == BO.orderStatus.Approved ? "Shipped" : "Delivered";
                estimatedFinishStatus = $"{(e.UserState as Tuple<int, Order>).Item1 / 1000} seconds.";
                ProgressStatus = "Updating...";


                break;
            case 2: //update done
                    ProgressStatus = $"{idLb.Content} updated successfully!";
                break;

            case 3: // simulator terminated
                    //idLb.Visibility = Visibility.Hidden;
                    //statusLb.Visibility = Visibility.Hidden;
                    //updateToLb.Visibility = Visibility.Hidden;
                    //estimatedFinishLb.Visibility = Visibility.Hidden;
                    //ProgressLb.Visibility = Visibility.Hidden;
                    //SimulatorTerminatedLb.Visibility = Visibility.Visible;

                break;
        }
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        simulator.Report += orderChanged1!;
        simulator.Init();
        while (isTimerRun)
        {
            timerworker.ReportProgress(0);
            Thread.Sleep(1000);
        }
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            ProgressLb.Content = "Terminated.";
        }
        //else if (e.Error != null)
        //{
        //    // e.Result throw System.Reflection.TargetInvocationException
        //    Console.WriteLine(e.Error.Message); //Exception Message
        //}
        //else
        //{
        //    long result = (long)e.Result;
        //    if (result < 1000)
        //        resultLabel.Content = "Done after " + result + " ms.";
        //    else
        //        resultLabel.Content = "Done after " + result / 1000 + " sec.";
        //}
    }

    private void orderChanged1(object sender, EventArgs e)
    {
        if ((e as TupleSimulatorArgs).state == 1) //update started
        {
            int delay = (e as TupleSimulatorArgs)!.delay;
            Order ord = (e as TupleSimulatorArgs)!.ord;
            Tuple<int, Order> tupleParams = new(delay, ord);
            timerworker.ReportProgress(1, tupleParams);
        }
        else
            timerworker.ReportProgress((e as TupleSimulatorArgs).state, e);
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        if (isTimerRun)
        {
            stopWatch.Stop();
            isTimerRun = false;
            simulator.Quit();
            Closing += (s, e) => e.Cancel = false;

            if (timerworker.WorkerSupportsCancellation == true)
                // Cancel the asynchronous operation.
                timerworker.CancelAsync();
        }
    }
}
