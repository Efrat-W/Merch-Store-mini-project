namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Linq;
using static Dal.DataSource;

internal class DalOrderItem : IOrderItem
{
    string path = "ordersItems.xml";
    string configPath = "config.xml";
    XElement orderItemsRoot;

    public DalOrderItem()
    {
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(path))
                orderItemsRoot = XElement.Load(@"xml/" + path);
            else
            {
                orderItemsRoot = new XElement("ordersItems");
                orderItemsRoot.Save(path);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("product File upload problem" + ex.Message);
        }
    }

    public int Create(OrderItem item)
    {
        //נחוץ? כי בגלל המספר הרץ אין סיכוי שיהיה קיים עוד אחד כמוהו לפני
        //OrderItem? itemCheck = OrderItems.Find(i => i?.ProductID == item.ProductID && i?.OrderID == item.OrderID);
        //if (itemCheck != null)
        // throw new MissingEntityException("Requested Order Item already exists.\n");

        //Read config file
        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("orderItemSeq").Value);
        nextSeqNum++;
        item.ID = nextSeqNum;
        //update config file
        configRoot.Element("orderItemSeq").SetValue(nextSeqNum);
        configRoot.Save(configPath);

        XElement Id = new XElement("Id", item.ID);
        XElement ProductID = new XElement("Product Id", item.ProductID);
        XElement OrderID = new XElement("Order Id", item.OrderID);
        XElement Price = new XElement("Price", item.Price);
        XElement Amount = new XElement("Amount", item.Amount);

        orderItemsRoot.Add(new XElement("OrderItem", Id, ProductID, OrderID, Price, Amount));
        orderItemsRoot.Save(path);
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
        XMLTools.SaveListToXMLSerializer(orderItems, path);
    }
    public void Delete(OrderItem item)
    {
        List<OrderItem?> OrderItems = XMLTools.LoadListFromXMLSerializer<OrderItem?>(path);
        OrderItem? itemToRemove = OrderItems.Find(i => i?.ID == item.ID) ?? throw new MissingEntityException("Requested Order Item does not exist.\n");
        OrderItems.Remove(itemToRemove);
        XMLTools.SaveListToXMLSerializer(orderItems, path);
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
