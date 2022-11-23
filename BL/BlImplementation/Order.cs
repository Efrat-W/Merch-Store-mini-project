using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using BO;
using Dal;
using DalApi;
using DO;

namespace BlImplementation;

internal class Order : IOrder
{
    IDal dal = new DalList();

    public IEnumerable<BO.OrderForList> RequestList()
    {
        IEnumerable<DO.Order> orders = dal.Order.RequestAll();
        Func<DO.Order, OrderForList> convert = OrderToOrderForList;
        IEnumerable<BO.OrderForList> orderlist = orders.Select(convert);
        return orderlist;
    }
    //help method:
    private OrderForList OrderToOrderForList(DO.Order ord)
    {
        orderStatus Status;
        if (DateTime.Now > ord.DeliveryDate)
            Status = orderStatus.Delivered;
        else if (DateTime.Now > ord.ShipDate)
            Status = orderStatus.Shipped;
        else
            Status=orderStatus.Approved;
        IEnumerable<DO.OrderItem> orderItems=dal.OrderItem.RequestAllItemsByOrderID(ord.ID);
        int amount=orderItems.Sum(item=>item.Amount);
        double totalPrice = orderItems.Sum(item => item.Price);
        return new OrderForList() { ID=ord.ID, CustomerName=ord.CustomerName,
        TotalPrice=totalPrice, AmountOfItems=amount, Status=};
    }

    public BO.Order RequestOrder(int id)
    {
        if (!(id > 99999 && id <= 999999))
            throw new ArgumentException("blahblah");

        DO.Order ord =dal.Order.RequestById(id);

        orderStatus Status;
        if (DateTime.Now > ord.DeliveryDate)
            Status = orderStatus.Delivered;
        else if (DateTime.Now > ord.ShipDate)
            Status = orderStatus.Shipped;
        else
            Status = orderStatus.Approved;

        IEnumerable<BO.OrderItem> items = dal.OrderItem.RequestAllItemsByOrderID(ord.ID).Select(DoOrderItemToBoOrderItem);
           
        return new BO.Order() { Id=id, CustomerName=ord.CustomerName,
        CustomerEmail=ord.CustomerEmail, CustomerAddress=ord.CustomerAddress,
        OrderDate=ord.OrderDate, ShipDate=ord.ShipDate, DeliveryDate=ord.DeliveryDate,
        Status=Status, Items= (List<BO.OrderItem>)items, TotalPrice=items.Sum(item=>item.Price)};
    }
    //help method:
    private BO.OrderItem DoOrderItemToBoOrderItem(DO.OrderItem item)
    {
        string name = dal.Product.RequestById(item.ProductID).Name;
        return new BO.OrderItem()
        {
            Name = name,
            ID = item.OrderID,
            ProductId = item.ProductID,
            Amount = item.Amount,
            Price = item.Price,
            TotalPrice = item.Price * item.Amount
        };
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
