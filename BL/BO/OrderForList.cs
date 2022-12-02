

using DO;
using System.Diagnostics;
using System.Xml.Linq;

namespace BO;

public class OrderForList
{
    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public orderStatus? Status { get; set; }
    public int AmountOfItems { get; set; }
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
    Product id: {ID}
    Order status: {Status}
    Customer Name: {CustomerName}
    Amount of items: {AmountOfItems}
    Total: {TotalPrice}";

}
