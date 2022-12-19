// See https://aka.ms/new-console-template for more information


using Dal;
using System.Data.Common;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");
XMLTools.SaveListToXMLSerializer(DataSource.products, "products.xml");
XMLTools.SaveListToXMLSerializer(DataSource.orders, "orders.xml");
XMLTools.SaveListToXMLSerializer(DataSource.orderItems, "orderItems.xml");

XElement configRoot = new XElement("config");
configRoot.Add(new XElement("orderSeq"), DataSource.Config.orderSeqID);
configRoot.Add(new XElement("orderItemSeq"), DataSource.Config.orderItemSeqID);
configRoot.Save("config.xml");