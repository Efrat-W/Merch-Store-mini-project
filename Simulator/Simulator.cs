using BO;
namespace Simulator;

enum SimulationProgress { UpdateDone=1, Done}
public static class Simulator
{
    private const int SEC = 1000;
    private static volatile bool _disposed = false;
    private static BlApi.IBl? bl = BlApi.Factory.Get();
    private static event EventHandler Report;
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
                    Report(tupleParams, EventArgs.Empty);

                    Thread.Sleep(delay);
                    if (ord.Status == orderStatus.Approved)
                        bl.Order.UpdateShipment(ord.Id);
                    else
                        bl.Order.UpdateDelivery(ord.Id);
                    Report(SimulationProgress.UpdateDone, EventArgs.Empty);
                }
                Thread.Sleep(SEC);
            }

            Report(SimulationProgress.Done, EventArgs.Empty);
        }).Start();
    }
    public static void Quit()
    {

    }
}