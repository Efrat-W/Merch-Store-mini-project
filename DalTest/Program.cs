// See https://aka.ms/new-console-template for more information
using Dal;
using DO;
using Microsoft.VisualBasic;
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
            DalProduct dalProduct = new DalProduct();
        do
        {
            //try
            //{
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                        dalProduct.Create(InitializeProduct());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine(dalProduct.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        List<Product> productList = dalProduct.RequestAll();
                        foreach (Product prod in productList)
                            Console.WriteLine(prod); ;
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing product's ID");
                        id = int.Parse(Console.ReadLine());
                        Product update = dalProduct.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Price, category and the amount in stock");
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
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
        } while (op != options.Return);
    }

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
            DalOrder dalOrder = new DalOrder();
        do
        {
            try
            {
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter your Name, Email and Adresss");
                        dalOrder.Create(InitializeOrder());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine(dalOrder.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        List<Order> orderList = dalOrder.RequestAll();
                        foreach (Order ord in orderList)
                            Console.WriteLine(ord); ;
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing order's ID");
                        id = int.Parse(Console.ReadLine());
                        Order update = dalOrder.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Email and Adresss");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } while (op != options.Return);
    }

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
            DalOrderItem dalOrderItem = new DalOrderItem();
        do
        {
            try
            {
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter product id, order id and amount ");
                        dalOrderItem.Create(InitializeOrderItem());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine(dalOrderItem.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        List<OrderItem> itemList = dalOrderItem.RequestAll();
                        foreach (OrderItem i in itemList)
                            Console.WriteLine(i);
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing order item's ID");
                        id = int.Parse(Console.ReadLine());
                        OrderItem update = dalOrderItem.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new product id, order id and amount");
                        dalOrderItem.Update(InitializeOrderItem());
                        break;
                    case options.DeleteFromList:
                        Console.WriteLine("Enter the ID of the order item you wish to remove ");
                        OrderItem item = new OrderItem() { ID = int.Parse(Console.ReadLine()) };
                        dalOrderItem.Delete(item);
                        break;
                    case options.ShowByProdAndOrder:
                        Console.WriteLine("Enter product id and order id:");
                        Product product = new Product() { ID = int.Parse(Console.ReadLine()) };
                        Order order = new Order() { ID = int.Parse(Console.ReadLine()) };
                        dalOrderItem.RequestByProductAndOrder(product, order);
                        break;
                    case options.ShowListOfProductsInOrder:
                        Console.WriteLine("Enter order id:");
                        int orderID = int.Parse(Console.ReadLine());
                        dalOrderItem.RequestAllItemsByOrderID(orderID);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } while (op != options.Return);
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

