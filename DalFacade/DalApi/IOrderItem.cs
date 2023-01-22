using DO;

namespace DalApi;
//Order item interface, inherrits the Icrud interface 
//and adds some more functions
public interface IOrderItem : ICrud<OrderItem>
{
    public OrderItem? RequestByProductAndOrder(Product? prod, Order? ord);

    public IEnumerable<OrderItem?> RequestAllItemsByOrderID(int i);
}