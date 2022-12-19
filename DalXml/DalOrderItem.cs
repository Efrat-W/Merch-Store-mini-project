namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Linq;


internal class DalOrderItem : IOrderItem
{
    string path = "orderItems.xml";
    string configPath = "config.xml";
    

    public int Create(OrderItem item)
    {
        List<OrderItem> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem>(path);

        //Read config file
        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("orderItemSeq")!.Value);
        nextSeqNum++;
        item.ID = nextSeqNum;
        //update config file
        configRoot.Element("orderItemSeq")!.SetValue(nextSeqNum);
        configRoot.Save(configPath);

        if (OrderItems.Exists(x => x.ID == item.ID))
            throw new MissingEntityException("Requested Order already exists.\n");

        OrderItems.Add(item);

        XMLTools.SaveListToXMLSerializer(OrderItems, path);

        return item.ID;
    }

    public IEnumerable<OrderItem?> RequestAll(Func<OrderItem?, bool>? func = null)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        if (func == null)
            return OrderItems.Select(o => o);
        return OrderItems.Where(o => func(o));
    }

    public OrderItem RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }

    public OrderItem RequestByFunc(Func<OrderItem?, bool>? func)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        return OrderItems.Find(o => func(o)) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
    }

    public void Update(OrderItem item)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        //if orderItem is not exist throw exception 
        OrderItem? itemToRemove = OrderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        OrderItems.Remove(itemToRemove);
        OrderItems.Add(item);
        XMLTools.SaveListToXMLSerializer(OrderItems, path);
    }
    public void Delete(OrderItem item)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        OrderItem? itemToRemove = OrderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        OrderItems.Remove(itemToRemove);
        XMLTools.SaveListToXMLSerializer(OrderItems, path);
    }

    public OrderItem? RequestByProductAndOrder(Product? prod, Order? ord)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        OrderItem? item = OrderItems.Find(i => i?.ProductID == prod?.ID && i?.OrderID == ord?.ID);
        if (item != null)
            throw new MissingEntityException("Requested Order Item does not exist.\n");
        return item;
    }
    public IEnumerable<OrderItem?> RequestAllItemsByOrderID(int ordID)
    {
        //Func<OrderItem?, bool>? func = (item) => { return item?.OrderID == ordID; };
        return RequestAll(item => item?.OrderID == ordID);
    }
}
