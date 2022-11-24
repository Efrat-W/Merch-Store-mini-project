

namespace BO;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public orderStatus Status { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public List<OrderItem> Items { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
    Customer Name: {CustomerName}
    Customer Email: {CustomerEmail}
    Customer Address: {CustomerAddress}
    Order Date: {OrderDate}
    Shipping Date: {ShipDate}
    Delivery Date: {DeliveryDate}
    Items in Cart: {foreach(OrderItem item in Items) item;} 
    Total Price: {TotalPrice}";

}
