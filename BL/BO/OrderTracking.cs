

namespace BO;

public class OrderTracking
{
    public int ID { get; set; }
    public orderStatus Status { get; set; }
    public List<Tuple<DateTime, string>> orderProgress { get; set; }
    public override string ToString()
    {
        string s = $@"
    Order id: {ID}
    Status: {Status}
    ";
        foreach (var tuple in orderProgress)
        {
            s += $"at {tuple.Item1}: {tuple.Item2}\n";
        }
        return s;
    }
}
