// See https://aka.ms/new-console-template for more information
using Dal;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DalTest;
internal class Program
{
    private DalProduct dalProduct = new DalProduct();
    private DalOrder dalOrder = new DalOrder();
    private DalOrderItem dalOrderItem = new DalOrderItem();
    static void Main(string[] args)
    {
        menu choice = new menu();
        do
        {
            Console.WriteLine(@"Enter one of the following:
                               0: Exit
                               1: Product
                               2: Order
                               3: Order Item");
            string input = Console.ReadLine();
            choice = (menu)Enum.Parse(typeof(menu), input);

            switch (choice)
            {
                case menu.Exit:
                    break;
                case menu.Product:
                    ProductMenu();
                    break;
                case menu.Order:
                    OrderMenu();
                    break;
                case menu.OrderItem:
                    OrderItemMenu();
                    break;
                default:
                    Console.WriteLine("you fool!");
                    break;
            }
        } while (choice != menu.Exit);


        void ProductMenu()
        {
            options op = new options();
            Console.WriteLine("uhh");
            string input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
            DalProduct dalProduct = new DalProduct();

            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                    dalProduct.Create(InitializeProduct());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int id = int.Parse(Console.ReadLine());
                    (dalProduct.RequestById(id)).ToString(); //print requested product to console
                    break;
                case options.ShowList:
                    List<Product> productList = dalProduct.RequestAll();
                    foreach (Product prod in productList)
                        prod.ToString();
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing product's ID, and new Name, Price, category or the amount in stock for it");
                    dalProduct.Update(InitializeProduct());
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the product you wish to remove ");
                    Product product = new Product() { ID = int.Parse(Console.ReadLine()) };
                    dalProduct.Delete(product);
                    break;
                default:
                    break;
            }
        }

        void OrderMenu()
        {
            options op = new options();
            Console.WriteLine("uhh");
            string input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
            DalOrder dalOrder = new DalOrder();

            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter your Name, Email and Adresss");
                    dalOrder.Create(InitializeOrder());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int id = int.Parse(Console.ReadLine());
                    (dalOrder.RequestById(id)).ToString(); //print requested product to console
                    break;
                case options.ShowList:
                    List<Order> orderList = dalOrder.RequestAll();
                    foreach (Order ord in orderList)
                        ord.ToString();
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order's ID, and new Name,Email or Adresss to it");
                    dalOrder.Update(InitializeOrder());
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order you wish to remove ");
                    Order order = new Order() { ID = int.Parse(Console.ReadLine()) };
                    dalOrder.Delete(order);
                    break;
                default:
                    break;
            }
        }

        void OrderItemMenu()
        {
            options op = new options();
            Console.WriteLine("uhh");
            string input = Console.ReadLine();
            op = (options)Enum.Parse(typeof(options), input);
            DalOrderItem dalOrderItem = new DalOrderItem();

            switch (op)
            {
                case options.Add:
                    Console.WriteLine("Enter your Name, Email and Adresss");
                    dalOrderItem.Create(InitializeOrderItem());
                    break;
                case options.ShowById:
                    Console.WriteLine("Enter ID");
                    int id = int.Parse(Console.ReadLine());
                    (dalOrderItem.RequestById(id)).ToString(); //print requested product to console
                    break;
                case options.ShowList:
                    List<OrderItem> itemList = dalOrderItem.RequestAll();
                    foreach (OrderItem i in itemList)
                        i.ToString();
                    break;
                case options.Update:
                    Console.WriteLine("Enter the existing order's ID, and new Name,Email or Adresss to it");
                    dalOrderItem.Update(InitializeOrderItem());
                    break;
                case options.DeleteFromList:
                    Console.WriteLine("Enter the ID of the order you wish to remove ");
                    OrderItem item = new OrderItem() { ID = int.Parse(Console.ReadLine()) };
                    dalOrderItem.Delete(item);
                    break;
                default:
                    break;
            }
        }

        static Product InitializeProduct()
        {
            int id = int.Parse(Console.ReadLine());
            string name = Console.ReadLine();
            int price = int.Parse(Console.ReadLine());
            category category = (category)Enum.Parse(typeof(category), Console.ReadLine());
            int inStock = int.Parse(Console.ReadLine());

            Product product = new Product() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
            return product;
        }

        static Order InitializeOrder()
        {
            string cusName = Console.ReadLine();
            string cusEmail = Console.ReadLine();
            string cusAddress = Console.ReadLine();
            DateTime orderDate = DateTime.Now; //?

            Order order = new Order() { CustomerAddress = cusAddress, CustomerEmail = cusEmail, CustomerName = cusName };
            return order;
        }

        static OrderItem InitializeOrderItem()
        {
            int prodID = int.Parse(Console.ReadLine());
            int ordID = int.Parse(Console.ReadLine());
            int amount = int.Parse(Console.ReadLine());
            DalProduct dalProduct = new DalProduct();
            double price = dalProduct.RequestById(prodID).Price;

            OrderItem item = new OrderItem() { ProductID = prodID, OrderID = ordID, Amount = amount, Price = price };
            return item;
        }
    }


}

