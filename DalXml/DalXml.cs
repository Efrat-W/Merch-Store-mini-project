using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal;

sealed internal class DalXml: IDal
{
    //lazy and thread safe singleton for saving the data in lists 
    
    private static readonly Lazy<DalXml> lazy = new(() => new DalXml());
    public static DalXml Instance { get { return lazy.Value; } }
    public IProduct Product { get; } = new DalProduct();
    public IOrder Order { get; } = new DalOrder();
    public IOrderItem OrderItem { get; } = new DalOrderItem();

    static string dir = @"..\xml\";
    string productsFilePath = @"products.xml";
    string ordersFilePath = @"ordersList.xml";
    string orderItemsFilePath = @"orderItems.xml";

    private DalXml()
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(dir + productsFilePath))
            Dal.XMLTools.SaveListToXMLSerializer<DO.Product?>(DataSource.products, dir + productsFilePath);

        if (!File.Exists(dir + ordersFilePath))
            Dal.XMLTools.SaveListToXMLSerializer<DO.Order?>(DataSource.orders, dir + ordersFilePath);

        if (!File.Exists(dir + orderItemsFilePath))
            Dal.XMLTools.SaveListToXMLSerializer<DO.OrderItem?>(DataSource.orderItems, dir + orderItemsFilePath);

    }

}
