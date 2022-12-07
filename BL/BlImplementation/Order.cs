
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
        IEnumerable<DO.Order?> orders = dal.Order.RequestAll();
        IEnumerable<OrderForList> orderlist = orders.Select(OrderToOrderForList);//converts to order for list
        return orderlist;
    }

    /// <summary>
    /// help method, converts DO order to BO order for list
    /// </summary>
    /// <param name="ord"></param>
    /// <returns></returns>
    private OrderForList OrderToOrderForList(DO.Order? ord)
    {
        //innitializes order status:
        orderStatus Status;
        if (ord?.DeliveryDate != null)
            Status = orderStatus.Delivered;
        else if (ord?.ShipDate != null)
            Status = orderStatus.Shipped;
        else
            Status=orderStatus.Approved;
        IEnumerable<DO.OrderItem?> orderItems=dal.OrderItem.RequestAllItemsByOrderID(ord?.ID ?? throw new MissingEntityException());//collects all items of this order
        int amount =orderItems.Sum(item=>item?.Amount ?? throw new MissingEntityException());
        double totalPrice = orderItems.Sum(item => item?.Price ?? throw new MissingEntityException());
        return new OrderForList() { 
            ID = ord?.ID ?? throw new MissingEntityException(), 
            CustomerName = ord?.CustomerName ?? throw new MissingEntityException(),
            TotalPrice = totalPrice,
            AmountOfItems = amount,
            Status = Status
        };
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
        DO.Order? ord;
        try
        { ord = dal.Order.RequestById(id); }
        catch (MissingEntityException ex) { throw new EntityNotFoundException("Requested Order not found.", ex); }
        //innitializes order status:
        orderStatus Status;
        if (ord?.DeliveryDate != null)
            Status = orderStatus.Delivered;
        else if (ord?.ShipDate != null)
            Status = orderStatus.Shipped;
        else
            Status = orderStatus.Approved;

        IEnumerable<BO.OrderItem> items;
        try
        {
            items = dal.OrderItem.RequestAllItemsByOrderID(ord?.ID ?? throw new InvalidArgumentException()).Select(DoOrderItemToBoOrderItem);//collects all items of this order and convert to BO
        }
        catch (MissingEntityException ex)
        {
            throw new InvalidArgumentException(ex);
        }
        return new BO.Order() {
            Id = id, 
            CustomerName=ord?.CustomerName,
            CustomerEmail=ord?.CustomerEmail, 
            CustomerAddress=ord?.CustomerAddress,
            OrderDate=ord?.OrderDate, 
            ShipDate=ord?.ShipDate, 
            DeliveryDate=ord?.DeliveryDate,
            Status=Status, 
            Items= (List<BO.OrderItem>)items, 
            TotalPrice=items.Sum(item=>item.Price * item.Amount)
        };
    }
    /// <summary>
    /// help method, converts DO order item to BO order item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    private BO.OrderItem DoOrderItemToBoOrderItem(DO.OrderItem? item)
    {
        string name;
        try
        {
            name = dal.Product.RequestById(item?.ProductID ?? throw new MissingEntityException()).Name!;//finds name
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException("Requested Product not found.", ex); }
        return new BO.OrderItem()
        {
            Name = name,
            ID = item?.OrderID ?? throw new MissingEntityException(),
            ProductId = item?.ProductID ?? throw new MissingEntityException(),
            Amount = item?.Amount ?? throw new MissingEntityException(),
            Price = item?.Price ?? throw new MissingEntityException(),
            TotalPrice = item?.Price * item?.Amount ?? throw new MissingEntityException()
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
        catch (MissingEntityException ex) { throw new InvalidArgumentException("Order to update delivery not found.", ex); }
        //updates status and ship date
        if (ord.ShipDate != null)
            throw new InvalidDateException("The order was already shipped.\n");
        ord.ShipDate = DateTime.Now;

        IEnumerable<BO.OrderItem> items = from DO.OrderItem item in dal.OrderItem.RequestAllItemsByOrderID(id)//collects all items of this order and convert to BO
                                          select DoOrderItemToBoOrderItem(item);
        try
        {
            dal.Order.Update(ord);//tries updating dl
        }
        catch (Exception ex) { throw new InvalidArgumentException("Requested Order to update shipment not found.", ex); }
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
            TotalPrice = items.Sum(item => item.Price * item.Amount)
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
        catch (Exception ex) { throw new InvalidArgumentException("Requested Order to update delivery not found.", ex); }
        //updates status and delivery date
        if (ord.DeliveryDate != null)
            throw new InvalidDateException("The order was already delivered.\n");
        else if (ord.ShipDate == null)
            throw new InvalidDateException("Order cannot be delivered without being shipped first.\n");
        ord.DeliveryDate = DateTime.Now;
        try
        {
            dal.Order.Update(ord);
        }
        catch (MissingEntityException ex) { throw new InvalidArgumentException("Order to update delivery not found.", ex); }
        IEnumerable<BO.OrderItem> items = dal.OrderItem.RequestAllItemsByOrderID(ord.ID).Select(DoOrderItemToBoOrderItem);//collects all items of this order and convert to BO
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
            TotalPrice = items.Sum(item => item.Price * item.Amount)
        };
    }
    /// <summary>
    /// returns order tracking entity according to given order id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public OrderTracking Track(int id)
    {
        DO.Order? ord;
        try
        {
            ord = dal.Order.RequestById(id);
        }
        catch (MissingEntityException ex) { throw new EntityNotFoundException("Order requested to track not found.", ex); }
        List<Tuple<DateTime?, string>>? tuples = new();
        orderStatus Status=0;
        //updates list of tuples according to order proccess
        if (ord?.OrderDate != null) {
            Tuple<DateTime?, string> tuple = new(ord?.OrderDate, "order placed");
            tuples.Add(tuple);
            Status = orderStatus.Approved;
            if (ord?.ShipDate != null) {
                tuples.Add(new(ord?.OrderDate, "order shipped"));
                Status = orderStatus.Shipped;
                if (ord?.DeliveryDate != null) {
                    tuples.Add(new(ord?.OrderDate, "order delivered"));
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


