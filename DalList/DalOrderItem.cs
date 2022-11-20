
using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

namespace Dal;

internal class DalOrderItem : IOrderItem
{
    /// <summary>
    /// adds the order item to the list of order items
    /// </summary>
    /// <param name="item">the new order item</param>
    /// <exception cref="Exception"></exception>
    public int Create(OrderItem item)
    {
        if (DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
            throw new DoubledEntityException();
        item.ID = Config.OrderItemSeqID;
        DataSource.orderItems.Add(item);
        return item.ID;
    }

    /// <summary>
    /// returns the list of order items
    /// </summary>
    /// <returns><list type="OrderItem">list of order items</returns>
    public IEnumerable<OrderItem> RequestAll()
    {
        List<OrderItem> itemList = new List<OrderItem>();
        foreach (OrderItem item in DataSource.orderItems)
            itemList.Add(item);
        return itemList;
    }

    /// <summary>
    /// returns the order item by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem RequestById(int id)
    {
        if (!DataSource.orderItems.Exists(i => i.ID == id))
            throw new MissingEntityException();
        return DataSource.orderItems.Find(i => i.ID == id);
    }

    /// <summary>
    ///  updates the order item with the same id to the given item's data
    /// </summary>
    /// <param name="item">the updated order item</param>
    /// <exception cref="Exception"></exception>
    public void Update(OrderItem item)
    {
        //if order is not exist throw exception 
        //if (!DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
        //    throw new Exception("cannot update, the order item does not exists");
        //OrderItem itemToRemove = DataSource.orderItems.Find(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID);
        if (!DataSource.orderItems.Exists(i => i.ID == item.ID))
            throw new MissingEntityException();
        OrderItem itemToRemove = DataSource.orderItems.Find(i => i.ID == item.ID);
        DataSource.orderItems.Remove(itemToRemove);
        DataSource.orderItems.Add(item);
    }

    /// <summary>
    /// deletes the order item from the list 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(OrderItem item)
    {
        if (!DataSource.orderItems.Exists(i => i.ProductID == item.ProductID && i.OrderID == item.OrderID))
            throw new MissingEntityException();
        OrderItem toRemove = RequestById(item.ID);
        DataSource.orderItems.Remove(toRemove);
    }

    /// <summary>
    /// ruturns the requested item by the produt and order it's belong to
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="ord"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem RequestByProductAndOrder(Product prod, Order ord)
    {
        if (!DataSource.orderItems.Exists(i => i.ProductID == prod.ID && i.OrderID == ord.ID))
            throw new MissingEntityException();
        return DataSource.orderItems.Find(i => i.ProductID == prod.ID && i.OrderID == ord.ID);
    }

    /// <summary>
    /// returns the requested item by its order id
    /// </summary>
    /// <param name="ordID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<OrderItem> RequestAllItemsByOrderID(int ordID)
    {
        if (!DataSource.orderItems.Exists(i => i.ID == ordID))
            throw new MissingEntityException();
        List<OrderItem> itemsInOrder = new List<OrderItem>();
        foreach (OrderItem item in orderItems)
        {
            if (item.OrderID == ordID)
                itemsInOrder.Add(item);
        }


        return itemsInOrder;
    }
}
