

namespace BO;

public class OrderTracking
{
    public int ID { get; set; }
    public orderStatus Status { get; set; }
    public List<Tuple<DateTime, string>> orderProgress { get; set; }
}
