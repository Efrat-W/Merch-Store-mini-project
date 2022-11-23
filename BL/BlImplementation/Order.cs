using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Order : IOrder
{
    IDal dal = new DalList();

    public IEnumerable<BO.OrderForList> RequestList()
    {
        IEnumerable<DO.Order> orders = dal.Order.RequestAll();
        IEnumerable<BO.OrderForList> orderlist;

        int amount = 0;
        from order in dal.Order.RequestAll()
        dal.OrderItem.RequestAllItemsByOrderID(order.ID);







        int amount = 0, price = 0;
        from order in dal.Order.RequestAll()
            from DO.OrderItem item in dal.OrderItem.RequestAllItemsByOrderID(order.ID)
                amount += dal.OrderItem.RequestAllItemsByOrderID(order.ID).Sum(item => item.)





        IEnumerable < DO.Order > orders = dal.Order.RequestAll();
        foreach (var order in orders)
        {
            dal.OrderItem.RequestAllItemsByOrderID(order.ID);

        }
             
    }






}

// Tool functions
static class OrderTools
{
    public static DO.Order OrderBoToDo(this BO.Order ord)
    {
        return new DO.Order()
        {
            ID = ord.Id,
            CustomerEmail = ord.CustomerEmail,
            CustomerAddress = ord.CustomerAddress,
            CustomerName = ord.CustomerName,
            OrderDate = ord.OrderDate,
            DeliveryDate = ord.DeliveryDate,
            ShipDate = ord.ShipDate
        };
    }

    public static BO.Order OrderDoToBo(this DO.Order ord)//////??
    {
        return new BO.Order()
        {
            Id = ord.ID,
            CustomerEmail = ord.CustomerEmail,
            CustomerAddress = ord.CustomerAddress,
            CustomerName = ord.CustomerName,
            OrderDate = ord.OrderDate,
            DeliveryDate = ord.DeliveryDate,
            ShipDate = ord.ShipDate
        };
    }
}
