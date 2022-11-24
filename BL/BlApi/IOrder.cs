
using BO;
namespace BlApi;

public interface IOrder
{
    public IEnumerable<OrderForList> RequestOrders();

    public Order RequestById(int id);
    
    public Order UpdateShipment(int id);

    public Order UpdateDelivery(int id);

    public OrderTracking Track(int id);
}