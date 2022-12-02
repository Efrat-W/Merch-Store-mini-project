
using DO;
using System.Diagnostics;

namespace BO;

public class OrderItem
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public int ProductId { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
    order item id: {ID}
    Product id: {ProductId}
    Product Name: {Name}
    Amount: {Amount}
    Price: {Price}
    Total: {TotalPrice}";
}
