﻿
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
    static private IDal dalList;
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
            string input = Console.ReadLine();
            choice = (menu)Enum.Parse(typeof(menu), input);
            
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
            
        string input = Console.ReadLine();
        op = (options)Enum.Parse(typeof(options), input);
        do
        {
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                        dalList.Product.Create(InitializeProduct());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine(dalList.Product.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        IEnumerable<Product> productList = dalList.Product.RequestAll();
                        foreach (Product prod in productList)
                            Console.WriteLine(prod); ;
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing product's ID");
                        id = int.Parse(Console.ReadLine());
                        Product update = dalList.Product.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Price, category and the amount in stock");
                        dalList.Product.Update(InitializeProduct(id));
                        break;
                    case options.DeleteFromList:
                        Console.WriteLine("Enter the ID of the product you wish to remove ");
                        Product product = new Product() { ID = int.Parse(Console.ReadLine()) };
                        dalList.Product.Delete(product);
                        break;
                    default:
                        break;
                }
            //re-ask for user input
            input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
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
        string input = Console.ReadLine();
        op = (options)Enum.Parse(typeof(options), input);
        do
        {
            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter your Name, Email and Adresss");
                    dalList.Order.Create(InitializeOrder());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    input = Console.ReadLine();
                    int.TryParse(input, out int id);
                    Console.WriteLine(dalList.Order.RequestById(id)); 
                    break;
                case options.ShowList:
                    IEnumerable<Order> orderList = dalList.Order.RequestAll();
                    foreach (Order ord in orderList)
                        Console.WriteLine(ord);
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order's ID");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Order update = dalList.Order.RequestById(id);
                    Console.WriteLine(update);
                    Console.WriteLine("Enter the new Name, Email and Adresss");
                    dalList.Order.Update(UpdateOrder(update));
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order you wish to remove ");
                    Order order = new Order() { ID = int.Parse(Console.ReadLine()) };
                    dalList.Order.Delete(order);
                    break;
                default:
                    break;
            }
            //re-ask for user input
            input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
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
        string input = Console.ReadLine();
        op = (options)Enum.Parse(typeof(options), input);
        do
        {
            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter product id, order id and amount ");
                    dalList.OrderItem.Create(InitializeOrderItem());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int id = int.Parse(Console.ReadLine());
                    Console.WriteLine(dalList.OrderItem.RequestById(id)); 
                    break;
                case options.ShowList:
                    IEnumerable<OrderItem> itemList = dalList.OrderItem.RequestAll();
                    foreach (OrderItem i in itemList)
                        Console.WriteLine(i);
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order item's ID");
                    id = int.Parse(Console.ReadLine());
                    OrderItem update = dalList.OrderItem.RequestById(id);
                    Console.WriteLine(update);
                    dalList.OrderItem.Update(UpdateOrderItem(update));
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order item you wish to remove ");
                    OrderItem item = new OrderItem() { ID = int.Parse(Console.ReadLine()) };
                    dalList.OrderItem.Delete(item);
                    break;
                case options.ShowByProdAndOrder:
                    Console.WriteLine("Enter product id and order id:");
                    Product product = new Product() { ID = int.Parse(Console.ReadLine()) };
                    Order order = new Order() { ID = int.Parse(Console.ReadLine()) };
                    dalList.OrderItem.RequestByProductAndOrder(product, order);
                    break;
                case options.ShowListOfProductsInOrder:
                    Console.WriteLine("Enter order id:");
                    input = Console.ReadLine();
                    int.TryParse(input, out int orderID);
                    IEnumerable<OrderItem> list = dalList.OrderItem.RequestAllItemsByOrderID(orderID);
                    foreach (OrderItem listItem in list)
                        Console.Write(listItem);
                    break;
                default:
                    break;
            }
            //re-ask for user input
            input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
        } while (op != options.Return);
    }


    /// <summary>
    /// reads the information from the user and innitializes the product
    /// </summary>
    /// <returns> the initialized product</returns>
    static Product InitializeProduct(int updateId = 0)
    {
        int id = updateId;
        string input;
        if (updateId == 0)
        {
            input = Console.ReadLine();
            int.TryParse(input, out id);
        }
        input = Console.ReadLine();
        string name = input;
        input = Console.ReadLine();
        int.TryParse(input, out int price);
        category category = (category)Enum.Parse(typeof(category), Console.ReadLine());
        input = Console.ReadLine();
        int.TryParse(input, out int inStock);

        Product product = new Product() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
        return product;
    }

    /// <summary>
    /// updates given order with the given id
    /// </summary>
    /// <param name="updateOrder">order object with the desired id</param>
    /// <returns>the requested order, updated</returns>
    static Order UpdateOrder(Order updateOrder)
    {
        string cusName = Console.ReadLine();
        string cusEmail = Console.ReadLine();
        string cusAddress = Console.ReadLine();
        Order order = new Order() { 
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
        string cusName = Console.ReadLine();
        string cusEmail = Console.ReadLine();
        string cusAddress = Console.ReadLine();
        DateTime orderDate = DateTime.Now; 

        Order order = new Order() { CustomerAddress = cusAddress, CustomerEmail = cusEmail, CustomerName = cusName };
        return order;
    }

    static OrderItem UpdateOrderItem(OrderItem item)
    { 
        Console.WriteLine("Enter the new product id and amount");
        string input = Console.ReadLine();
        int.TryParse(input, out int prodID);
        input = Console.ReadLine();
        int.TryParse(input, out int amount);
        double price = dalList.Product.RequestById(prodID).Price;

        OrderItem updatedItem = new() { ID = item.ID, Amount = amount, OrderID = item.OrderID, Price = price, ProductID = prodID };
        return updatedItem;


        //dalList.Product dalList.Product = new();
        //dalList.Order dalList.Order = new();

        //string input = Console.ReadLine();
        //int.TryParse(input, out int prodID);
        //input = Console.ReadLine();
        //int.TryParse(input, out int ordID);

        //Product prod = dalList.Product.RequestById(prodID);
        //Order ord = dalList.Order.RequestById(ordID);

        //updatedItem = new OrderItem() { };


        //string input = Console.ReadLine();
        //int.TryParse(input, out int prodID);
        //input = Console.ReadLine();
        //int.TryParse(input, out int ordID);
        //input = Console.ReadLine();
        //int.TryParse(input, out int amount);
        ////dalList.Product dalList.Product = new dalList.Product();
        ////double price = dalList.Product.RequestById(prodID).Price;
        //Product prod = new();
        //Order ord = new();
        //prod.ID = prodID;
        //ord.ID = ordID;
        //dalList.Product dalList.Product = new();
        //dalList.Order dalList.Order = new();
        //dalList.Product.Create(prod);
        //dalList.Order.Create(ord);
        //double price = dalList.Product.RequestById(prodID).Price;
        //OrderItem updatedItem = new OrderItem() { ID = item.ID, ProductID = prodID, OrderID = ordID, Amount = amount, Price = price 
    }

    /// <summary>
    /// reads the information from the user and innitializes the order item
    /// </summary>
    /// <returns>the initialized order item</returns>
    static OrderItem InitializeOrderItem()
    {
        string input = Console.ReadLine();
        int.TryParse(input, out int prodID);
        input = Console.ReadLine();
        int.TryParse(input, out int ordID);
        input = Console.ReadLine();
        int.TryParse(input, out int amount);
        double price = dalList.Product.RequestById(prodID).Price;

        OrderItem item = new OrderItem() { ProductID = prodID, OrderID = ordID, Amount = amount, Price = price };
        return item;
    }
}

