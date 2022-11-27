using BlApi;
using BlImplementation;
using BO;
using Dal;
using DO;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlTest
{
    internal class Program
    {
        static private IBl bl = new Bl();
        static void Main(string[] args)
        {
            BO.menu choice = new BO.menu();
            do
            {
                Program program = new Program();
                Console.WriteLine(@"Enter one of the following:
    0: Exit
    1: Product
    2: Order
    3: Cart");
                string input = Console.ReadLine();
                choice = (BO.menu)Enum.Parse(typeof(BO.menu), input);

                switch (choice)
                {
                    case BO.menu.Product:
                        program.ProductMenu();
                        break;
                    case BO.menu.Order:
                        program.OrderMenu();
                        break;
                    case BO.menu.Cart:
                        program.CartMenu();
                        break;
                    default:
                        break;
                }
            } while (choice != BO.menu.Exit);
        }

        private void ProductMenu()
        {
            BO.optionsProduct op = new BO.optionsProduct();
            Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: add product
    2: print product (by id) for manager
    3: print product (by id) for customer
    4: print all products
    5: update a product
    6: delete a product");

            string input = Console.ReadLine();
            op = (BO.optionsProduct)Enum.Parse(typeof(BO.optionsProduct), input);
            do
            {
                switch (op)
                {
                    case BO.optionsProduct.Add:
                        Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                        try { bl.Product.Add(InitializeProduct()); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); ; }
                        break;
                    case BO.optionsProduct.ShowByIdMan:
                        Console.WriteLine("Enter ID");
                        int.TryParse(Console.ReadLine(), out int id);
                        try { Console.WriteLine(bl.Product.RequestByIdManager(id)); } //print requested product to console
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case BO.optionsProduct.ShowByIdCus:
                        Console.WriteLine("Enter ID");
                        int.TryParse(Console.ReadLine(), out id);
                        Console.WriteLine("Enter customer name, email, adress and the number of items in your cart:");
                        Console.WriteLine(bl.Product.RequestByIdCustomer(id, InitializeCart())); 
                        break;
                    case BO.optionsProduct.RequestAll:
                        IEnumerable<BO.ProductForList> list = bl.Product.RequestList();
                        foreach (BO.ProductForList item in list)
                            Console.WriteLine(item);
                        break;
                    case BO.optionsProduct.Update:
                        Console.WriteLine("Enter the existing product's ID");
                        id = int.Parse(Console.ReadLine());
                        BO.Product update = bl.Product.RequestByIdManager(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Price, category and the amount in stock");
                        bl.Product.Update(InitializeProduct(id));
                        break;
                    case BO.optionsProduct.Delete:
                        Console.WriteLine("Enter the ID of the product you wish to remove ");
                        int.TryParse(Console.ReadLine(), out id);
                        bl.Product.Delete(id);
                        break;
                    default:
                        break;
                }
                //re-ask for user input
                input = Console.ReadLine();
                op = (BO.optionsProduct)Enum.Parse(typeof(BO.optionsProduct), input);
            } while (op != BO.optionsProduct.Return);
        }
        private void OrderMenu()
        {
            BO.optionsOrder op = new BO.optionsOrder();
            Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: track order
    2: print order (by id)
    3: update shipment
    4: update delivery
    5: print all orders");
            string input = Console.ReadLine();
            op = (BO.optionsOrder)Enum.Parse(typeof(BO.optionsOrder), input);
            do
            {
                switch (op)
                {
                    case BO.optionsOrder.Track:
                        Console.WriteLine("Enter id:");
                        input = Console.ReadLine();
                        int.TryParse(input, out int id);
                        Console.WriteLine(bl.Order.Track(id));
                        break;
                    case BO.optionsOrder.ShowById:
                        Console.WriteLine("Enter ID");
                        input = Console.ReadLine();
                        int.TryParse(input, out  id);
                        Console.WriteLine(bl.Order.RequestById(id));
                        break;
                    case BO.optionsOrder.UpdateShipment:
                        Console.WriteLine("Enter ID");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        Console.WriteLine(bl.Order.UpdateShipment(id));
                        break;
                    case BO.optionsOrder.UpdateDelivery:
                        Console.WriteLine("Enter ID");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        Console.WriteLine(bl.Order.UpdateDelivery(id));
                        break;
                    case BO.optionsOrder.RequestAll:
                        IEnumerable<BO.OrderForList> list = bl.Order.RequestOrders();
                        foreach (BO.OrderForList item in list)
                            Console.WriteLine(item);
                        break;
                    default:
                        break;
                }
                //re-ask for user input
                input = Console.ReadLine();
                op = (BO.optionsOrder)Enum.Parse(typeof(BO.optionsOrder), input);
            } while (op != BO.optionsOrder.Return);
        }

        private void CartMenu()
        {
            BO.optionsCart op = new BO.optionsCart();
            Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: add product
    2: change product's amount
    3: approve the order");
            string input = Console.ReadLine();
            op = (BO.optionsCart)Enum.Parse(typeof(BO.optionsCart), input);
            do
            {
                switch (op)
                {
                    case BO.optionsCart.Add:
                        Console.WriteLine("Enter product's id:");
                        input = Console.ReadLine();
                        int.TryParse(input, out int id);
                        Console.WriteLine("Enter customer name, email, address and the number of items in your cart:");
                        Console.WriteLine(bl.Cart.AddProduct(InitializeCart(),id));
                        break;
                    case BO.optionsCart.UpdateAmount:
                        Console.WriteLine("Enter product's id and the new amount (- or +)");
                        input = Console.ReadLine();
                        int.TryParse(input, out int prodId);
                        input = Console.ReadLine();
                        int.TryParse(input, out int amount);
                        Console.WriteLine("Enter customer name, email, adress and the number of items in your cart:");
                        Console.WriteLine(bl.Cart.UpdateProductAmount(InitializeCart(),prodId, amount));
                        break;
                    case BO.optionsCart.Approve:
                        Console.WriteLine("Enter customer name, email, adress and the number of items in your cart:");
                        Console.WriteLine(bl.Cart.Approve(InitializeCart()));
                        break;
                    default:
                        break;
                }
                //re-ask for user input
                input = Console.ReadLine();
                op = (BO.optionsCart)Enum.Parse(typeof(BO.optionsCart), input);
            } while (op != BO.optionsCart.Return);
        }

        static BO.Product InitializeProduct(int updateId = 0)
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
            BO.category category;
            try
            {
                category = (BO.category)Enum.Parse(typeof(BO.category), Console.ReadLine());
            }
            catch (Exception ex){ throw new InvalidArgumentException("category", ex); }
            input = Console.ReadLine();
            int.TryParse(input, out int inStock);

            return new BO.Product() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
        }
        static BO.Cart InitializeCart()
        {
            string input = Console.ReadLine();
            string name = input;
            input = Console.ReadLine();
            string email = input;
            input = Console.ReadLine();
            string adress = input;
            int num;
            input = Console.ReadLine();
            int.TryParse(input, out num);
            List<BO.OrderItem> Items= new List<BO.OrderItem>();
            Console.WriteLine("Now enter the products in your cart:\n");
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("Enter name, product id, price and amount");
                Items.Add(InitializeOrderItem());
            }
            double total = Items.Sum(i => i.Price * i.Amount);
            return new BO.Cart() { 
                CustomerName=name,
                CustomerEmail=email,
                CustomerAddress=adress,
                Items=Items,
                TotalPrice=total,
            };           
        }
        static BO.OrderItem InitializeOrderItem()
        {
            int prodId, amount;
            double price;
            string input = Console.ReadLine();
            string name = input;
            input = Console.ReadLine();
            int.TryParse(input, out prodId);
            input = Console.ReadLine();
            double.TryParse(input, out price);
            input = Console.ReadLine();
            int.TryParse(input, out amount);
            
           return new BO.OrderItem()
            {
               Name = name,
               ProductId = prodId,
               Price = price,   
               Amount = amount,
               TotalPrice=amount*price,
            };
        }
    }
}
