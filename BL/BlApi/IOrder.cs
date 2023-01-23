
using BO;
namespace BlApi;
//Order interrface
public interface IOrder
{
    public IEnumerable<OrderForList> RequestOrders();

    public Order RequestById(int id);
    
    public Order UpdateShipment(int id);

    public Order UpdateDelivery(int id);

    public OrderTracking Track(int id);

    public Order GetOldest();
}