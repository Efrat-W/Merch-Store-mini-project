

namespace BO;

public class Cart
{
    public String CustomerName { get; set; }
    public String CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public List<OrderItem> Items { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() =>  $@"
    Customer Name: {CustomerName}
    Customer Email: {CustomerEmail}
    Customer Address: {CustomerAddress}
    Items in Cart: {Items.ToString()}
    Total Price: {TotalPrice}";
}
