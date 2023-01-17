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

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        string timerText = stopWatch.Elapsed.ToString();
        TimerText = timerText.Substring(0, 8);
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        while (isTimerRun)
        {
            timerworker.ReportProgress(1);
            Thread.Sleep(1000);
        }
    }
    private Stopwatch stopWatch;
    private bool isTimerRun;
    BackgroundWorker timerworker;
    public SimulationWindow()
    {
        InitializeComponent();
        stopWatch = new Stopwatch();
        timerworker = new BackgroundWorker();
        timerworker.DoWork += Worker_DoWork;
        timerworker.ProgressChanged += Worker_ProgressChanged;
        timerworker.WorkerReportsProgress = true;
    }
}
