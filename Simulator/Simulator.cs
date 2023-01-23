using BO;
using System.ComponentModel;

namespace Simulator;
/// <summary>
/// simulator class (for updating order progress)
/// </summary>
public static class simulator
{
    private const int SEC = 1000;
    private static volatile bool _disposed = false;
    private static BlApi.IBl? bl = BlApi.Factory.Get();
    public static event EventHandler? Report;
    public static event Action? EndSimulator;
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
                    Report!(Thread.CurrentThread, new TupleSimulatorArgs(delay, ord)); //update init

                    Thread.Sleep(delay);
                    if (ord.Status == orderStatus.Approved)
                        bl.Order.UpdateShipment(ord.Id);
                    else
                        bl.Order.UpdateDelivery(ord.Id);
                    if (!_disposed)
                        Report(Thread.CurrentThread, new TupleSimulatorArgs(2)); //update done
                }
                else //no more orders to update
                {
                    Quit();
                    Report(Thread.CurrentThread, new TupleSimulatorArgs(3));
                }
                Thread.Sleep(SEC);
            }
        }).Start();
    }
    public static void Quit()
    {
        _disposed = true;
        EndSimulator();
    }
}