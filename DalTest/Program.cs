
using Dal;
using DalApi;
using DO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DalTest;
internal class Program
{
    static private IDal? dalList = Factory.Get();
    static void Main(string[] args)
    {
        menu choice = new menu();
        do
        {
            Program program = new Program();
            Console.WriteLine(@"Enter one of the following:
    0: Exit
    1: Product
    2: Order
    3: Order Item");
            choice = (menu)Enum.Parse(typeof(menu), Console.ReadLine()!);
            
            switch (choice)
            {
                case menu.Product:
                    program.ProductMenu();
                    break;
                case menu.Order:
                    program.OrderMenu();
                    break;
                case menu.OrderItem:
                    program.OrderItemMenu();
                    break;
                default:
                    break;
            }
        } while (choice != menu.Exit);
    }
    /// <summary>
    /// operates the product menu
    /// </summary>
    private void ProductMenu()
    {
        options op = new options();
        Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: add product
    2: print product (by id)
    3: print the list of products
    4: update a certain product
    5: delete a product");
        op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        int id;
        do
        {
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                        dalList?.Product.Create(InitializeProduct());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int.TryParse(Console.ReadLine(), out id);
                        Console.WriteLine(dalList?.Product.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        IEnumerable<Product?> productList = dalList!.Product.RequestAll();
                        foreach (Product? prod in productList)
                            Console.WriteLine(prod); ;
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing product's ID");
                        int.TryParse(Console.ReadLine(), out id);
                        Product? update = dalList?.Product.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Price, category and the amount in stock");
                        dalList!.Product.Update(InitializeProduct(id));
                        break;
                    case options.DeleteFromList:
                        Console.WriteLine("Enter the ID of the product you wish to remove ");
                        Product product = new() { ID = int.Parse(Console.ReadLine()!) };
                        dalList?.Product.Delete(product);
                        break;
                    default:
                        break;
                }
            //re-ask for user input
            op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        } while (op != options.Return);
    }
    /// <summary>
    /// operates the order menu
    /// </summary>
    private void OrderMenu()
    {
        options op = new options();
        Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: add a new order
    2: print order (by id)
    3: print the list of orders
    4: update a certain order
    5: delete an order");
        op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        do
        {
            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter your Name, Email and Adresss");
                    dalList?.Order.Create(InitializeOrder());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int.TryParse(Console.ReadLine(), out int id);
                    Console.WriteLine(dalList?.Order.RequestById(id)); 
                    break;
                case options.ShowList:
                    IEnumerable<Order?> orderList = dalList?.Order.RequestAll();
                    foreach (Order? ord in orderList!)
                        Console.WriteLine(ord);
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order's ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Order update = dalList!.Order.RequestById(id);
                    Console.WriteLine(update);
                    Console.WriteLine("Enter the new Name, Email and Adresss");
                    dalList.Order.Update(UpdateOrder(update));
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order you wish to remove ");
                    Order order = new() { ID = int.Parse(Console.ReadLine()!) };
                    dalList?.Order.Delete(order);
                    break;
                default:
                    break;
            }
            //re-ask for user input
            op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        } while (op != options.Return);
    }
    /// <summary>
    /// operates the order items menu
    /// </summary>
    void OrderItemMenu()
    {
        options op = new options();
        Console.WriteLine(@"Enter one of the following:
        0: return to main menu
        1: add order item
        2: print order item (by id)
        3: print the list of order items
        4: update a certain order item
        5: delete an order item
        6: print order item (by product and order)
        7: print all the items in a certain order"); 
        op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        int id;
        do
        {
            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter product id, order id and amount ");
                    dalList?.OrderItem.Create(InitializeOrderItem());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(dalList?.OrderItem.RequestById(id)); 
                    break;
                case options.ShowList:
                    IEnumerable<OrderItem?> itemList = dalList!.OrderItem.RequestAll();
                    foreach (OrderItem? i in itemList)
                        Console.WriteLine(i);
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order item's ID");
                    int.TryParse(Console.ReadLine(), out id);
                    OrderItem update = dalList!.OrderItem.RequestById(id);
                    Console.WriteLine(update);
                    dalList.OrderItem.Update(UpdateOrderItem(update));
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order item you wish to remove ");
                    OrderItem item = new() { ID = int.Parse(Console.ReadLine()!) };
                    dalList?.OrderItem.Delete(item);
                    break;
                case options.ShowByProdAndOrder:
                    Console.WriteLine("Enter product id and order id:");
                    Product product = new() { ID = int.Parse(Console.ReadLine()!) };
                    Order order = new() { ID = int.Parse(Console.ReadLine()!) };
                    dalList?.OrderItem.RequestByProductAndOrder(product, order);
                    break;
                case options.ShowListOfProductsInOrder:
                    Console.WriteLine("Enter order id:");
                    int.TryParse(Console.ReadLine(), out int orderID);
                    IEnumerable<OrderItem?> list = dalList!.OrderItem.RequestAllItemsByOrderID(orderID);
                    foreach (OrderItem listItem in list!)
                        Console.Write(listItem);
                    break;
                default:
                    break;
            }
            //re-ask for user input
            op = (options)Enum.Parse(typeof(options), Console.ReadLine()!);
        } while (op != options.Return);
    }


    /// <summary>
    /// reads the information from the user and innitializes the product
    /// </summary>
    /// <returns> the initialized product</returns>
    static Product InitializeProduct(int updateId = 0)
    {
        int id = updateId;
        if (updateId == 0)
            int.TryParse(Console.ReadLine(), out id);
        string name = Console.ReadLine()!;
        int.TryParse(Console.ReadLine(), out int price);
        category category = (category)Enum.Parse(typeof(category), Console.ReadLine()!);
        int.TryParse(Console.ReadLine(), out int inStock);

        Product product = new() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
        return product;
    }

    /// <summary>
    /// updates given order with the given id
    /// </summary>
    /// <param name="updateOrder">order object with the desired id</param>
    /// <returns>the requested order, updated</returns>
    static Order UpdateOrder(Order updateOrder)
    {
        string cusName = Console.ReadLine()!;
        string cusEmail = Console.ReadLine()!;
        string cusAddress = Console.ReadLine()!;
        Order order = new() { 
            ID = updateOrder.ID, CustomerAddress = cusAddress, CustomerEmail = cusEmail, CustomerName = cusName,
            DeliveryDate = updateOrder.DeliveryDate, OrderDate = updateOrder.OrderDate, ShipDate = updateOrder.ShipDate};
        return order;
    }

    /// <summary>
    /// reads the information from the user and innitializes the order
    /// </summary>
    /// <returns>the initialized order</returns>
    static Order InitializeOrder()
    {
        string cusName = Console.ReadLine()!;
        string cusEmail = Console.ReadLine()!;
        string cusAddress = Console.ReadLine()!;
        DateTime orderDate = DateTime.Now; 

        Order order = new() { CustomerAddress = cusAddress, CustomerEmail = cusEmail, CustomerName = cusName };
        return order;
    }

    static OrderItem UpdateOrderItem(OrderItem item)
    { 
        Console.WriteLine("Enter the new product id and amount");
        int.TryParse(Console.ReadLine(), out int prodID);
        int.TryParse(Console.ReadLine(), out int amount);
        double price = dalList!.Product.RequestById(prodID).Price;

        OrderItem updatedItem = new() { ID = item.ID, Amount = amount, OrderID = item.OrderID, Price = price, ProductID = prodID };
        return updatedItem;
    }

    /// <summary>
    /// reads the information from the user and innitializes the order item
    /// </summary>
    /// <returns>the initialized order item</returns>
    static OrderItem InitializeOrderItem()
    {
        int.TryParse(Console.ReadLine(), out int prodID);
        int.TryParse(Console.ReadLine(), out int ordID);
        int.TryParse(Console.ReadLine(), out int amount);
        double price = dalList!.Product.RequestById(prodID).Price;

        OrderItem item = new() { ProductID = prodID, OrderID = ordID, Amount = amount, Price = price };
        return item;
    }
}

