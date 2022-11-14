
using System.Xml.Linq;

namespace DO;

public struct OrderItem
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    public override string ToString() => $@"
        Item ID: {ID}
        Product ID: {ProductID}
        Order ID: {OrderID}
        Price: {Price}
        Amount: {Amount}
";
}
