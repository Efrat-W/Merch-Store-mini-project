using BO;
using System.ComponentModel;

namespace Simulator;

enum SimulationProgress { UpdateDone=1, Done}
public static class simulator
{
    private const int SEC = 1000;
    private static volatile bool _disposed = false;
    private static BlApi.IBl? bl = BlApi.Factory.Get();
    public static BackgroundWorker Report = new();
    private static int delay = 0;
    private static Random rand = new();
    public static void Init()
    {
        new Thread(() =>
        {
            while (!_disposed)
            {
                Order ord = bl!.Order.GetOldest();
                if (ord != null)
                {
                    delay = rand.Next(2, 10) * SEC;
                    Tuple<int, Order> tupleParams = new(delay, ord);
                    Report.ReportProgress(0 ,tupleParams);

                    Thread.Sleep(delay);
                    if (ord.Status == orderStatus.Approved)
                        bl.Order.UpdateShipment(ord.Id);
                    else
                        bl.Order.UpdateDelivery(ord.Id);
                    Report.ReportProgress(0, SimulationProgress.UpdateDone);
                }
                Thread.Sleep(SEC);
            }
            Report.ReportProgress(0, SimulationProgress.Done);
        }).Start();
    }
    public static void Quit()
    {

    }
}