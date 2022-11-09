
using DO;
using static Dal.DataSource;

namespace Dal;

public class DalOrder
{
    public int Create(Order order)
    {
        if (DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("order already exists");
        order.ID = Config.OrderSeqID;
        DataSource.orders.Add(order);
        return order.ID;
    }

    public List<Order> RequestAll()
    {
        List<Order> orderList = new List<Order>();
        foreach (Order order in DataSource.orders)
            orderList.Add(order);
        return orderList;
    }

    public Order RequestById(int id)
    {
        if (!DataSource.orders.Exists(i => i.ID == id))
            throw new Exception("the order does not exist");
        return DataSource.orders.Find(i => i.ID == id);
    }

    public void Update(Order order)
    {
        //if order is not exist throw exception 
        if (!DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("cannot update, the order does not exists");
        Order orderToRemove = DataSource.orders.Find(i => i.ID == order.ID);
        DataSource.orders.Remove(orderToRemove);
        DataSource.orders.Add(order);
    }

    public void Delete(Order order)
    {
        //if student exist throw exception 
        if (!DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("cannot delete, order does not exists");
        DataSource.orders.Remove(order); //or set Active..
    }
}
