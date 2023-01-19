using BO;
using System.ComponentModel;

namespace Simulator;

enum SimulationProgress { UpdateDone=1, Done}
public static class simulator
{
    private const int SEC = 1000;
    private static volatile bool _disposed = false;
    private static BlApi.IBl? bl = BlApi.Factory.Get();
    public static event EventHandler? Report;
    private static int delay = 0;
    private static Random rand = new();
    public static void Init()
    {
        _disposed = false;
        new Thread(() =>
        {
            while (!_disposed)
            {
                Order ord = bl!.Order.GetOldest();
                if (ord != null)
                {
                    delay = rand.Next(2, 10) * SEC;
                    Report!(Thread.CurrentThread, new TupleSimulatorArgs(delay, ord));

                    Thread.Sleep(delay);
                    if (ord.Status == orderStatus.Approved)
                        bl.Order.UpdateShipment(ord.Id);
                    else
                        bl.Order.UpdateDelivery(ord.Id);
                    //Report(SimulationProgress.UpdateDone, EventArgs.Empty);
                    //Report!(Thread.CurrentThread, new );
                }
                Thread.Sleep(SEC);
            }
            //Report(SimulationProgress.Done, EventArgs.Empty);
        }).Start();
    }
    public static void Quit()
    {
        _disposed = true;
    }
}