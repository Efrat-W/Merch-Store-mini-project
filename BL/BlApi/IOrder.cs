
using BO;
namespace BlApi;

public interface IOrder
{
    public IEnumerable<Order> RequestAll();

    public Order RequestById(int id);

    public Order UpdateShipment(int id);

    public Order UpdateDelivery(int id);

    public Order Track(int id);

    public Order UpdateOrder(int id);
}