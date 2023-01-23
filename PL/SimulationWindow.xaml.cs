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
     private string TimerText
    {
        get { return (string)GetValue(TimerTextProperty); }
        set { SetValue(TimerTextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TimerText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimerTextProperty =
        DependencyProperty.Register("TimerText", typeof(string), typeof(SimulationWindow));

    private int IdLbContent
    {
        get { return (int)GetValue(IdLbContentProperty); }
        set { SetValue(IdLbContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IdLbContentProperty =
        DependencyProperty.Register("IdLbContent", typeof(int), typeof(SimulationWindow));

    private string StatusContent
    {
        get { return (string)GetValue(StatusContentProperty); }
        set { SetValue(StatusContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusContentProperty =
        DependencyProperty.Register("StatusContent", typeof(string), typeof(SimulationWindow));

    private string StatusUpdatedContent
    {
        get { return (string)GetValue(StatusUpdatedContentProperty); }
        set { SetValue(StatusUpdatedContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IdLbContent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusUpdatedContentProperty =
        DependencyProperty.Register("StatusUpdatedContent", typeof(string), typeof(SimulationWindow));

    private string estimatedFinishStatus
    {
        get { return (string)GetValue(estimatedFinishStatusProperty); }
        set { SetValue(estimatedFinishStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for estimatedFinishStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty estimatedFinishStatusProperty =
        DependencyProperty.Register("estimatedFinishStatus", typeof(string), typeof(SimulationWindow));

    private string ProgressStatus
    {
        get { return (string)GetValue(ProgressStatusProperty); }
        set { SetValue(ProgressStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProgressStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressStatusProperty =
        DependencyProperty.Register("ProgressStatus", typeof(string), typeof(SimulationWindow));

    public bool isEndOfSimulation
    {
        get { return (bool)GetValue(isEndOfSimulationProperty); }
        set { SetValue(isEndOfSimulationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for isEndOfSimulation.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty isEndOfSimulationProperty =
        DependencyProperty.Register("isEndOfSimulation", typeof(bool), typeof(SimulationWindow));



    private Stopwatch stopWatch;
    private bool isTimerRun;
    private bool canClose = false;
    BackgroundWorker backgroundWorker;
    public SimulationWindow()
    {
        //default values
        IdLbContent = 0;
        StatusContent = StatusUpdatedContent = "Loading...";
        isEndOfSimulation=false;
        //
        InitializeComponent();
        Closing +=SimulatorWindow_Closing!;

        stopWatch = new Stopwatch();
        stopWatch.Start();

        backgroundWorker = new BackgroundWorker();
        backgroundWorker.DoWork += Worker_DoWork!;
        backgroundWorker.ProgressChanged += Worker_ProgressChanged!;
        backgroundWorker.RunWorkerCompleted += Worker_RunWorkerCompleted!;
        backgroundWorker.WorkerReportsProgress = true;
        backgroundWorker.WorkerSupportsCancellation = true;
        isTimerRun = true;
        backgroundWorker.RunWorkerAsync();
    }
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        switch (e.ProgressPercentage)
        {
            case 0: //timer
                string timerText = stopWatch.Elapsed.ToString();
                TimerText = timerText.Substring(0, 8);
                break;
            case 1: //a new order is being updated
                IdLbContent = (e.UserState as Tuple<int, Order>).Item2.Id;
                StatusContent = (e.UserState as Tuple<int, Order>).Item2.Status.ToString();
                StatusUpdatedContent = (e.UserState as Tuple<int, Order>).Item2.Status == BO.orderStatus.Approved ? "Shipped" : "Delivered";
                estimatedFinishStatus = $"{(e.UserState as Tuple<int, Order>).Item1 / 1000} seconds.";
                ProgressStatus = "Updating...";
                break;
            case 2: //update done
                ProgressStatus = $"{idLb.Content} updated successfully!";
                break;
            default:
                break;

        }
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        simulator.Report += orderChanged1!;
        simulator.EndSimulator += cancelAsync;
        simulator.Init();
        while (isTimerRun)
        {
            backgroundWorker.ReportProgress(0);
            Thread.Sleep(1000);
        }
    }
   
    private void SimulatorWindow_Closing(object sender, CancelEventArgs e)
    {
        e.Cancel = !canClose;
    }

    private void cancelAsync()
    {
        backgroundWorker.CancelAsync();
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        simulator.Report -= orderChanged1!;
        simulator.EndSimulator -= cancelAsync;
        stopWatch.Stop();
        backgroundWorker.CancelAsync();
        isEndOfSimulation=true;
        ProgressStatus = "End of simulation.";
        canClose =true;

    }

    private void orderChanged1(object sender, EventArgs e)
    {
        if ((e as TupleSimulatorArgs).state == 1) //update started
        {
            int delay = (e as TupleSimulatorArgs)!.delay;
            Order ord = (e as TupleSimulatorArgs)!.ord;
            Tuple<int, Order> tupleParams = new(delay, ord);
            backgroundWorker.ReportProgress(1, tupleParams);
        }
        else
            backgroundWorker.ReportProgress((e as TupleSimulatorArgs).state, 0);
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        isTimerRun = false;
        simulator.Quit();
    }
}
