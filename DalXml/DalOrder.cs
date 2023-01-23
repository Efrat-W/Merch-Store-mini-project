using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;
using System.Runtime.CompilerServices;


namespace Dal;
internal class DalOrder : IOrder
{
    string path = "../xml/orders.xml";
    string configPath = "../xml/config.xml";
    /// <summary>
    /// adds the order to the list of orders
    /// </summary>
    /// <param name="order">the order to add</param>
    /// <returns></returns>
    /// <exception cref="Exception"> </exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Order Ord) {
        //read the orders list from the xml file
        List<Order> orders = XMLTools.LoadListFromXMLSerializer<Order>(path);

        //read the last number fron config file
        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("seqOrder")!.Value);
        nextSeqNum++;
        Ord.ID = nextSeqNum;
        //update config file
        configRoot.Element("seqOrder")!.SetValue(nextSeqNum);
        configRoot.Save(configPath);

        if (orders.Exists(x => x.ID == Ord.ID))
            throw new MissingEntityException("Requested Order already exists.\n");

        orders.Add(Ord);

        XMLTools.SaveListToXMLSerializer(orders, path);

        return Ord.ID;
    }
    /// <summary>
    /// returns the list of orders
    /// </summary>
    /// <returns><list type="Order">list of orders</list></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> RequestAll(Func<Order?, bool>? func = null)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        if (func == null)
            return orders;
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
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
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
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
        orders.Add(order);
        XMLTools.SaveListToXMLSerializer(orders, path);
    }
    /// <summary>
    /// deletes the order from the list 
    /// </summary>
    /// <param name="order"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(Order order)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
        XMLTools.SaveListToXMLSerializer(orders, path);
    }
}