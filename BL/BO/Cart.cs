

namespace BO;

public class Cart
{
    public String CustomerName { get; set; }
    public String CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public List<OrderItem> Items { get; set; }
    public double TotalPrice { get; set; }
}
