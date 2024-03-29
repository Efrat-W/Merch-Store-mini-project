﻿using System.Runtime.CompilerServices;
using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

internal class DalOrder : IOrder
{
    /// <summary>
    /// adds the order to the list of orders
    /// </summary>
    /// <param name="order">the order to add</param>
    /// <returns></returns>
    /// <exception cref="Exception"> </exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Order order)
    {
        //check if the order exist
        Order? orderCheck = orders.Find(i => i?.ID == order.ID);
        if (orderCheck != null)
            throw new DoubledEntityException("Requested Order already exists.\n");

        Order? newOrder = new()
        {
            ID = Config.OrderSeqID,
            CustomerName=order.CustomerName,
            CustomerAddress=order.CustomerAddress,
            CustomerEmail=order.CustomerEmail,
            DeliveryDate=order.DeliveryDate,
            OrderDate=order.OrderDate,
            ShipDate=order.ShipDate,
        };
        DataSource.orders.Add(newOrder);
        return newOrder?.ID ?? throw new MissingEntityException();
    }

    /// <summary>
    /// returns the list of orders
    /// </summary>
    /// <returns><list type="Order">list of orders</list></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> RequestAll(Func<Order?, bool>? func = null)
    {
        if (func == null)
            return orders.Select(o => o);
        return orders.Where(o => func(o));
    }

    /// <summary>
    /// returns the order by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Order</returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }

    /// <summary>
    /// returns the order by given func condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order RequestByFunc(Func<Order?, bool>? func = null)
    {
        if (func == null) throw new MissingEntityException("No filter condition was given.\n");
        return orders.Find(o => func(o)) ?? throw new MissingEntityException("Requested Order does not exist.\n");
    }

    /// <summary>
    /// updates the order with the same id to the given order's data
    /// </summary>
    /// <param name="order">the updated order</param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order order)
    {
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
        orders.Add(order);
    }

    /// <summary>
    /// deletes the order from the list 
    /// </summary>
    /// <param name="order"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(Order order)
    {
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
    }
}
