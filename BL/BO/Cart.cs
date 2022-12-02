

namespace BO;

public class Cart
{
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAddress { get; set; }
    public List<OrderItem?>? Items { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString()
    {
        string s = $@"
    Customer Name: {CustomerName}
    Customer Email: {CustomerEmail}
    Customer Address: {CustomerAddress}";
        foreach (OrderItem item in Items)
        {
            s += $" {item}";
        }
        s += $"\n\tTotal price: {TotalPrice}";
        return s;
    }
}
