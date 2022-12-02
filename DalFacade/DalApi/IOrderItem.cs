using DO;

namespace DalApi;

public interface IOrderItem : ICrud<OrderItem>
{
    public OrderItem? RequestByProductAndOrder(Product? prod, Order? ord);

    public IEnumerable<OrderItem?> RequestAllItemsByOrderID(int i);
}