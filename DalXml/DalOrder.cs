namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    string path = "orders.xml";
    string configPath = "config.xml";
    XElement ordersRoot;
    public DalOrder()
    {
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(path))
                ordersRoot = XElement.Load(@"xml/"+ path);
            else
            {
                ordersRoot = new XElement("orders");
                ordersRoot.Save(path);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("product File upload problem" + ex.Message);
        }
    }
    public int Create(Order Ord)
    {
        //Read config file
        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("orderSeq").Value);
        nextSeqNum++;
        Ord.ID = nextSeqNum;
        //update config file
        configRoot.Element("orderSeq").SetValue(nextSeqNum);
        configRoot.Save(configPath);

        XElement Id = new XElement("Id", Ord.ID);
        XElement CustomerName = new XElement("CustomerName", Ord.CustomerName);
        XElement CustomerEmail = new XElement("CustomerEmail", Ord.CustomerEmail);
        XElement CustomerAdress = new XElement("CustomerAdress", Ord.CustomerAddress);
        XElement OrderDate = new XElement("OrderDate", Ord.OrderDate);
        XElement ShipDate = new XElement("ShipDate", Ord.ShipDate);
        XElement DeliveryDate = new XElement("DeliveryDate", Ord.DeliveryDate);

        ordersRoot.Add(new XElement("Order", Id, CustomerName, CustomerEmail, CustomerAdress, OrderDate, ShipDate, DeliveryDate));
        ordersRoot.Save(path);

        return Ord.ID;
    }
    public IEnumerable<Order?> RequestAll(Func<Order?, bool>? func = null)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        if (func == null)
            return orders;
        return orders.Where(o => func(o));
    }

    public Order RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }

    public Order RequestByFunc(Func<Order?, bool>? func = null)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        if (func == null) throw new MissingEntityException("No filter condition was given.\n");
        return orders.Find(o => func(o)) ?? throw new MissingEntityException("Requested Order does not exist.\n");
    }

    public void Update(Order order)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
        orders.Add(order);
        XMLTools.SaveListToXMLSerializer(orders, path);
    }

    public void Delete(Order order)
    {
        List<Order?> orders = XMLTools.LoadListFromXMLSerializer<Order?>(path);
        Order? orderToRemove = orders.Find(i => i?.ID == order.ID) ?? throw new MissingEntityException("Requested Order does not exist.\n");
        orders.Remove(orderToRemove);
        XMLTools.SaveListToXMLSerializer(orders, path);
    }
}