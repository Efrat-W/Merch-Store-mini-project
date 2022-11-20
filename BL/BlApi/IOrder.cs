
using BO;
namespace BlApi;

public interface IOrder
{
    public IEnumerable<Order> RequestOrders();

    public Order RequestById(int id);

    public Order UpdateShip(int id);

    public Order UpdateDelivered(int id);

    public Order Track(int id);

    public Order UpdateOrder(int id);
}