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

internal class Order : BlApi.IOrder
{
    IDal dal = new DalList();

    public IEnumerable<BO.OrderForList> RequestOrders()
    {
        IEnumerable<DO.Order> orders = dal.Order.RequestAll();
        Func<DO.Order, OrderForList> convert = OrderToOrderForList;
        IEnumerable<BO.OrderForList> orderlist = orders.Select(convert);
        return orderlist;
    }
    //auxillary method:
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
        TotalPrice=totalPrice, AmountOfItems=amount, Status=Status};
    }

    public BO.Order RequestById(int id)
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

    public BO.Order UpdateShipment(int id)
    {
        DO.Order ord = dal.Order.RequestById(id);
        if (ord.ShipDate > DateTime.MinValue)
            throw new Exception("already sent");
        ord.ShipDate = DateTime.Now;
        IEnumerable<BO.OrderItem> items = dal.OrderItem.RequestAllItemsByOrderID(ord.ID).Select(DoOrderItemToBoOrderItem);
        return new BO.Order()
        {
            Id = id,
            CustomerName = ord.CustomerName,
            CustomerEmail = ord.CustomerEmail,
            CustomerAddress = ord.CustomerAddress,
            OrderDate = ord.OrderDate,
            ShipDate = ord.ShipDate,
            DeliveryDate = ord.DeliveryDate,
            Status = orderStatus.Shipped,
            Items = (List<BO.OrderItem>)items,
            TotalPrice = items.Sum(item => item.Price)
        };
    }

    public BO.Order UpdateDelivery(int id)
    {
        DO.Order ord = dal.Order.RequestById(id);
        if (ord.DeliveryDate > DateTime.MinValue)
            throw new Exception("already delivered");
        ord.DeliveryDate = DateTime.Now;
        IEnumerable<BO.OrderItem> items = dal.OrderItem.RequestAllItemsByOrderID(ord.ID).Select(DoOrderItemToBoOrderItem);
        return new BO.Order()
        {
            Id = id,
            CustomerName = ord.CustomerName,
            CustomerEmail = ord.CustomerEmail,
            CustomerAddress = ord.CustomerAddress,
            OrderDate = ord.OrderDate,
            ShipDate = ord.ShipDate,
            DeliveryDate = ord.DeliveryDate,
            Status = orderStatus.Delivered,
            Items = (List<BO.OrderItem>)items,
            TotalPrice = items.Sum(item => item.Price)
        };
    }

    public BO.OrderTracking Track(int id)
    {
        DO.Order ord = dal.Order.RequestById(id);
        List<Tuple<DateTime, string>> tuples= new List<Tuple<DateTime, string>>();
        orderStatus Status=0;
        if (ord.OrderDate > DateTime.MinValue) {
            Tuple<DateTime, string> tuple = new(ord.OrderDate, "order placed");
            tuples.Add(tuple);
            Status = orderStatus.Approved;
            if (ord.ShipDate > DateTime.MinValue) {
                tuples.Add(new(ord.OrderDate, "order shipped"));
                Status = orderStatus.Shipped;
                if (ord.DeliveryDate > DateTime.MinValue) {
                    tuples.Add(new(ord.OrderDate, "order delivered"));
                    Status = orderStatus.Delivered;
                }
            }
        }
        return new BO.OrderTracking()
        {
            ID = id,
            Status = Status,
            orderProgress = tuples
        };
    }
}


