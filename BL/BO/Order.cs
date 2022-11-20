

namespace BO;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public String CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public orderStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public List<OrderItem> Items { get; set; }
    public double TotalPrice { get; set; }

}
