﻿
using DO;
using static Dal.DataSource;

namespace Dal;

public class DalOrder
{
    /// <summary>
    /// adds the order to the list of orders
    /// </summary>
    /// <param name="order">the order to add  </param>
    /// <returns></returns>
    /// <exception cref="Exception"> </exception>
    public int Create(Order order)
    {
        order.ID = Config.OrderSeqID;
        if (DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("order already exists");
        DataSource.orders.Add(order);
        return order.ID;
    }

    /// <summary>
    /// returns the list of orders
    /// </summary>
    /// <returns><list type="Order">list of orders</list></returns>
    public List<Order> RequestAll()
    {
        List<Order> orderList = new List<Order>();
        foreach (Order order in DataSource.orders)
            orderList.Add(order);
        return orderList;
    }

    /// <summary>
    /// returns the order by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Order</returns>
    /// <exception cref="Exception"></exception>
    public Order RequestById(int id)
    {
        if (!DataSource.orders.Exists(i => i.ID == id))
            throw new Exception("the order does not exist");
        return DataSource.orders.Find(i => i.ID == id);
    }

    /// <summary>
    /// updates the order with the same id to the given order's data
    /// </summary>
    /// <param name="order">the updated order</param>
    /// <exception cref="Exception"></exception>
    public void Update(Order order)
    { 
        if (!DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("cannot update, the order does not exists");
        Order orderToRemove = DataSource.orders.Find(i => i.ID == order.ID);
        DataSource.orders.Remove(orderToRemove);
        DataSource.orders.Add(order);
    }

    /// <summary>
    /// deletes the order from the list 
    /// </summary>
    /// <param name="order"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(Order order)
    {
        if (!DataSource.orders.Exists(i => i.ID == order.ID))
            throw new Exception("cannot delete, order does not exists");
        Order toRemove = RequestById(order.ID);
        if(!DataSource.orders.Remove(toRemove))
            throw new Exception("cannot delete due to unknown error");
    }
}
