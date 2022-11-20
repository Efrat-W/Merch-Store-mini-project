
namespace BO;

public class OrderItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }
}
