namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    string path = "products.xml";
    string configPath = "config.xml";

    public int Create(Order Ord) { 
        List<Order> orders = XMLTools.LoadListFromXMLSerializer<Order>(path);

        XElement configRoot = XElement.Load(configPath);

        int nextSeqNum = Convert.ToInt32(configRoot.Element("orderSeq").Value);
        nextSeqNum++;
        Ord.ID = nextSeqNum;
        //update config file
        configRoot.Element("orderSeq").SetValue(nextSeqNum);
        configRoot.Save(configPath);

        if (orders.Exists(x => x.ID == Ord.ID))
            throw new MissingEntityException("Requested Order already exists.\n");

        orders.Add(Ord);

        XMLTools.SaveListToXMLSerializer(orders, path);

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