
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

namespace Dal;

internal class DalOrderItem
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public int Create(OrderItem item)
    {
        if (DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
            throw new Exception("order item already exists");
        item.ID = Config.OrderItemSeqID;
        DataSource.orderItems.Add(item);
        return item.ID;
    }

    public List<OrderItem> RequestAll()
    {
        List<OrderItem> itemList = new List<OrderItem>();
        foreach (OrderItem item in DataSource.orderItems)
            itemList.Add(item);
        return itemList;
    }

    public OrderItem RequestById(int id)
    {
        if (!DataSource.orderItems.Exists(i => i.ID == id))
            throw new Exception("the order item does not exist");
        return DataSource.orderItems.Find(i => i.ID == id);
    }

    public void Update(OrderItem item)
    {
        //if order is not exist throw exception 
        if (!DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
            throw new Exception("cannot update, the order item does not exists");
        OrderItem itemToRemove = DataSource.orderItems.Find(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID);
        DataSource.orderItems.Remove(itemToRemove);
        DataSource.orderItems.Add(item);
    }

    public void Delete(OrderItem item)
    {
        if (!DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
            throw new Exception("cannot delete, order item does not exists");
        DataSource.orderItems.Remove(item); //or set inActive..
    }

    public OrderItem RequestByProductAndOrder(Product prod, Order ord)
    {
        if (!DataSource.orderItems.Exists(i => i.ProductID == prod.ID && i.OrderID == ord.ID))
            throw new Exception("the order item does not exist");
        return DataSource.orderItems.Find(i => i.ProductID == prod.ID && i.OrderID == ord.ID);
    }


    public List<OrderItem> RequestAllItemsByOrderID(int ordID)
    {
        if (!DataSource.orderItems.Exists(i => i.ID == ordID))
            throw new Exception("no items of the given order id exist, unfortunately");
        List<OrderItem> itemsInOrder = new List<OrderItem>();
        foreach (OrderItem item in orderItems)
        {
            if (item.OrderID == ordID)
                itemsInOrder.Add(item);
        }


        return itemsInOrder;
    }
}
