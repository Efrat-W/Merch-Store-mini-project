
using BO;
using Dal;
using DalApi;
using DO;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    IDal dal = new DalList();
    /// <summary>
    /// returns list of orders
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.OrderForList> RequestOrders()
    {
        IEnumerable<DO.Order> orders = dal.Order.RequestAll();
        Func<DO.Order, OrderForList> convert = OrderToOrderForList;//delegate
        IEnumerable<BO.OrderForList> orderlist = orders.Select(convert);
        return orderlist;
    }
    /// <summary>
    /// help method, converts DO order to BO order for list
    /// </summary>
    /// <param name="ord"></param>
    /// <returns></returns>
    private OrderForList OrderToOrderForList(DO.Order ord)
    {
        orderStatus Status;
        if (DateTime.MinValue < ord.DeliveryDate)
            Status = orderStatus.Delivered;
        else if (DateTime.MinValue < ord.ShipDate)
            Status = orderStatus.Shipped;
        else
            Status=orderStatus.Approved;
        IEnumerable<DO.OrderItem> orderItems=dal.OrderItem.RequestAllItemsByOrderID(ord.ID);
        int amount=orderItems.Sum(item=>item.Amount);
        double totalPrice = orderItems.Sum(item => item.Price);
        return new OrderForList() { ID=ord.ID, CustomerName=ord.CustomerName,
        TotalPrice=totalPrice, AmountOfItems=amount, Status=Status};
    }
    /// <summary>
    /// returns the requested order by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Order RequestById(int id)
    {
        if (!(id > 99999 && id <= 999999))
           throw new InvalidArgumentException("Requested id is out of range.\n");
        DO.Order ord;
        try
        { ord = dal.Order.RequestById(id); }
        catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
        orderStatus Status;
        if (DateTime.MinValue < ord.DeliveryDate)
            Status = orderStatus.Delivered;
        else if (DateTime.MinValue < ord.ShipDate)
            Status = orderStatus.Shipped;
        else
            Status = orderStatus.Approved;

        IEnumerable<BO.OrderItem> items;
        try
        {
            items = dal.OrderItem.RequestAllItemsByOrderID(ord.ID).Select(DoOrderItemToBoOrderItem);
        }
        catch (MissingEntityException ex)
        {
            throw new InvalidArgumentException(ex);
        }
        return new BO.Order() {
            Id=id, 
            CustomerName=ord.CustomerName,
            CustomerEmail=ord.CustomerEmail, 
            CustomerAddress=ord.CustomerAddress,
            OrderDate=ord.OrderDate, 
            ShipDate=ord.ShipDate, 
            DeliveryDate=ord.DeliveryDate,
            Status=Status, 
            Items= (List<BO.OrderItem>)items, 
            TotalPrice=items.Sum(item=>item.Price)
        };
    }
    /// <summary>
    /// help method, converts DO order item to BO order item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    private BO.OrderItem DoOrderItemToBoOrderItem(DO.OrderItem item)
    {
        string name;
        try
        {
            name = dal.Product.RequestById(item.ProductID).Name;
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
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
    /// <summary>
    /// updates shipment date to current time, and status to shipped
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    /// <exception cref="InvalidDateException"></exception>
    public BO.Order UpdateShipment(int id)
    {
        DO.Order ord;
        try
        {
            ord = dal.Order.RequestById(id);
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
        if (ord.ShipDate > DateTime.MinValue)
            throw new InvalidDateException("The order was already shipped.\n");
        ord.ShipDate = DateTime.Now;
        IEnumerable<BO.OrderItem> items = from DO.OrderItem item in dal.OrderItem.RequestAllItemsByOrderID(id)
                                          select DoOrderItemToBoOrderItem(item);
        try
        {
            dal.Order.Update(ord);
        }
        catch (Exception ex) { throw new InvalidArgumentException(ex); }
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
            Items =items,
            TotalPrice = items.Sum(item => item.Price)
        };
    }
    /// <summary>
    /// updates delivery date to current time, and status to delivered
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    /// <exception cref="InvalidDateException"></exception>
    public BO.Order UpdateDelivery(int id)
    {
        DO.Order ord;
        try
        {
            ord = dal.Order.RequestById(id);
        }
        catch (Exception ex) { throw new InvalidArgumentException(ex); }
        if (ord.DeliveryDate > DateTime.MinValue)
            throw new InvalidDateException("The order was already delivered");
        else if (ord.ShipDate <= DateTime.MinValue)
            throw new InvalidDateException("Order cannot be delivered without being shipped first.\n");
        ord.DeliveryDate = DateTime.Now;
        try
        {
            dal.Order.Update(ord);
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
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
            Items = items,
            TotalPrice = items.Sum(item => item.Price)
        };
    }
    /// <summary>
    /// returns order tracking entity according to given order id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.OrderTracking Track(int id)
    {
        DO.Order ord;
        try
        {
            ord = dal.Order.RequestById(id);
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
        List<Tuple<DateTime, string>> tuples = new List<Tuple<DateTime, string>>();
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


