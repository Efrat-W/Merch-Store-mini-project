
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
        OrderItem? itemCheck = orderItems.Find(i => i?.ProductID == item.ProductID && i?.OrderID == item.OrderID );
        if (itemCheck != null)
            throw new MissingEntityException("Requested Order Item already exists.\n");
        OrderItem newItem = new() {
            ID = Config.OrderItemSeqID,
            ProductID = (int)itemCheck?.ProductID,
            OrderID = (int)itemCheck?.OrderID,
            Price = (int)itemCheck?.Price,
            Amount = (int)itemCheck?.Amount
        };
        orderItems.Add(item);
        return newItem.ID;
    }

    /// <summary>
    /// returns the list of order items
    /// </summary>
    /// <returns><list type="OrderItem">list of order items</returns>
    public IEnumerable<OrderItem?> RequestAll(Func<OrderItem?, bool>? func = null)
    {
        if (func == null)
            return orderItems.Select(o => o);
        return orderItems.Where(o => func(o));
    }

    /// <summary>
    /// returns the order item by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }

    /// <summary>
    /// returns the order item by given func condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public OrderItem RequestByFunc(Func<OrderItem?, bool>? func)
    {
        return orderItems.Find(o => func(o)) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
    }

    /// <summary>
    ///  updates the order item with the same id to the given item's data
    /// </summary>
    /// <param name="item">the updated order item</param>
    /// <exception cref="Exception"></exception>
    public void Update(OrderItem item)
    {
        //if orderItem is not exist throw exception 
        OrderItem? itemToRemove = orderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        orderItems.Remove(itemToRemove);
        orderItems.Add(item);
    }

    /// <summary>
    /// deletes the order item from the list 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(OrderItem item)
    {
        OrderItem? itemToRemove = orderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        orderItems.Remove(itemToRemove);
    }

    /// <summary>
    /// ruturns the requested item by the produt and order it's belong to
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="ord"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem? RequestByProductAndOrder(Product? prod, Order? ord)
    {
        OrderItem? item = orderItems.Find(i => i?.ProductID == prod?.ID && i?.OrderID == ord?.ID);
        if (item != null)
            throw new MissingEntityException("Requested Order Item does not exist.\n");
        return item;
    }

    /// <summary>
    /// returns the requested item by its order id
    /// </summary>
    /// <param name="ordID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<OrderItem?> RequestAllItemsByOrderID(int ordID)
    {
        //Func<OrderItem?, bool>? func = (item) => { return item?.OrderID == ordID; };
        return RequestAll(item => item?.OrderID == ordID);
    }
}
