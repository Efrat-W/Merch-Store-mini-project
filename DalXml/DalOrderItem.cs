namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

internal class DalOrderItem : IOrderItem
{
    string path = "../xml/orderItems.xml";
    string configPath = "../xml/config.xml";

    /// <summary>
    /// adds the order item to the list of order items
    /// </summary>
    /// <param name="item">the new order item</param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(OrderItem item)
    {
        //read the order item list from the xml file
        List<OrderItem> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem>(path);

        //read the last number fron config file
        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("seqOrderItem")!.Value);
        nextSeqNum++;
        item.ID = nextSeqNum;
        //update config file
        configRoot.Element("seqOrderItem")!.SetValue(nextSeqNum);
        configRoot.Save(configPath);

        if (OrderItems.Exists(x => x.ID == item.ID))
            throw new MissingEntityException("Requested Order already exists.\n");

        OrderItems.Add(item);

        XMLTools.SaveListToXMLSerializer(OrderItems, path);

        return item.ID;
    }
    /// <summary>
    /// returns the list of order items
    /// </summary>
    /// <returns><list type="OrderItem">list of order items</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> RequestAll(Func<OrderItem?, bool>? func = null)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        if (func == null)
            return OrderItems.Select(o => o);
        return OrderItems.Where(o => func(o));
    }
    /// <summary>
    /// returns the order item by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }
    /// <summary>
    /// returns the order item by given func condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem RequestByFunc(Func<OrderItem?, bool>? func)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        return OrderItems.Find(o => func!(o)) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
    }
    /// <summary>
    ///  updates the order item with the same id to the given item's data
    /// </summary>
    /// <param name="item">the updated order item</param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(OrderItem item)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        //if orderItem is not exist throw exception 
        OrderItem? itemToRemove = OrderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        OrderItems.Remove(itemToRemove);
        OrderItems.Add(item);
        XMLTools.SaveListToXMLSerializer(OrderItems, path);
    }
    /// <summary>
    /// deletes the order item from the list 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(OrderItem item)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        OrderItem? itemToRemove = OrderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        OrderItems.Remove(itemToRemove);
        XMLTools.SaveListToXMLSerializer(OrderItems, path);
    }
    /// <summary>
    /// ruturns the requested item by the produt and order it's belong to
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="ord"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem? RequestByProductAndOrder(Product? prod, Order? ord)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        OrderItem? item = OrderItems.Find(i => i?.ProductID == prod?.ID && i?.OrderID == ord?.ID);
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
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> RequestAllItemsByOrderID(int ordID)
    {
        return RequestAll(item => item?.OrderID == ordID);
    }
}
