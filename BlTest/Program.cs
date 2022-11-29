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
        static Cart myCart = null;
        static void Main(string[] args)
        {
            BO.menu choice = new BO.menu();
            do
            {
                Program program = new();
                Console.WriteLine(@"Enter one of the following:
    0: Exit
    1: Product
    2: Order
    3: Cart");
                choice = (BO.menu)Enum.Parse(typeof(BO.menu), Console.ReadLine());
                try
                {
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
                            throw new InvalidArgumentException("Invalid menu choice.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Exception thrown from BO interface:\n{ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException);
                }
            } while (choice != BO.menu.Exit);
        }
        /// <summary>
        ///operates all product's options
        /// </summary>
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
            op = (BO.optionsProduct)Enum.Parse(typeof(BO.optionsProduct), Console.ReadLine());
            do
            {
                try
                {
                    switch (op)
                    {
                        case BO.optionsProduct.Add:
                            Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                            bl.Product.Add(InitializeProduct());
                            break;
                        case BO.optionsProduct.ShowByIdMan:
                            Console.WriteLine("Enter ID");
                            int.TryParse(Console.ReadLine(), out int id);
                            Console.WriteLine(bl.Product.RequestByIdManager(id)); 
                            break;
                        case BO.optionsProduct.ShowByIdCus:
                            Console.WriteLine("Enter ID");
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine(bl.Product.RequestByIdCustomer(id, InitializeCart()));
                            break;
                        case BO.optionsProduct.RequestAll:
                            IEnumerable<BO.ProductForList> list = bl.Product.RequestList();
                            foreach (BO.ProductForList item in list)
                                Console.WriteLine(item);
                            break;
                        case BO.optionsProduct.Update:
                            Console.WriteLine("Enter the existing product's ID");
                            int.TryParse(Console.ReadLine(), out id);
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
                        case BO.optionsProduct.Return:
                            break;
                        default:
                            throw new InvalidArgumentException("Invalid menu choice.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Exception thrown from BO interface:\n{ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException);
                }
                //re-ask for user input
                op = (BO.optionsProduct)Enum.Parse(typeof(BO.optionsProduct), Console.ReadLine());
            } while (op != BO.optionsProduct.Return);
        }
        /// <summary>
        ///operates all order's options
        /// </summary>
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
            op = (BO.optionsOrder)Enum.Parse(typeof(BO.optionsOrder), Console.ReadLine());
            do
            {
                try
                {
                    switch (op)
                    {
                        case BO.optionsOrder.Track:
                            Console.WriteLine("Enter id:");
                            int.TryParse(Console.ReadLine(), out int id);
                            Console.WriteLine(bl.Order.Track(id));
                            break;
                        case BO.optionsOrder.ShowById:
                            Console.WriteLine("Enter ID");
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine(bl.Order.RequestById(id));
                            break;
                        case BO.optionsOrder.UpdateShipment:
                            Console.WriteLine("Enter ID");
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine(bl.Order.UpdateShipment(id));
                            break;
                        case BO.optionsOrder.UpdateDelivery:
                            Console.WriteLine("Enter ID");
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine(bl.Order.UpdateDelivery(id));
                            break;
                        case BO.optionsOrder.RequestAll:
                            IEnumerable<BO.OrderForList> list = bl.Order.RequestOrders();
                            foreach (BO.OrderForList item in list)
                                Console.WriteLine(item);
                            break;
                        case BO.optionsOrder.Return:
                            break;
                        default:
                            throw new InvalidArgumentException("Invalid menu choice.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Exception thrown from BO interface:\n{ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException);
                }
                //re-ask for user input
                op = (BO.optionsOrder)Enum.Parse(typeof(BO.optionsOrder), Console.ReadLine());
            } while (op != BO.optionsOrder.Return);
        }
        /// <summary>
        ///operates all cart's options
        /// </summary>
        private void CartMenu()
        {
            InitializeCart();
            BO.optionsCart op = new BO.optionsCart();
            Console.WriteLine(@"Enter one of the following:
    0: return to main menu
    1: add product
    2: change product's amount
    3: delete a product from cart
    4: empty cart
    5: approve the order");
            
            do
            { 
                op = (BO.optionsCart)Enum.Parse(typeof(BO.optionsCart), Console.ReadLine());
                try
                {
                    switch (op)
                    {
                        case BO.optionsCart.Add:
                            Console.WriteLine("Enter product's id:");
                            int.TryParse(Console.ReadLine(), out int id);
                            bl.Product.RequestByIdManager(id);
                            Console.WriteLine(bl.Cart.AddProduct(myCart, id));
                            break;
                        case BO.optionsCart.UpdateAmount:
                            Console.WriteLine("Enter product's id and the new amount (- or +)");
                            int.TryParse(Console.ReadLine(), out int prodId);
                            int.TryParse(Console.ReadLine(), out int amount);
                            Console.WriteLine(bl.Cart.UpdateProductAmount(myCart, prodId, amount));
                            break;
                        case BO.optionsCart.DeleteProduct:
                            Console.WriteLine("Enter product id:");
                            int.TryParse(Console.ReadLine(), out int prodid);
                            bl.Cart.UpdateProductAmount(myCart, prodid, 0);
                            break;
                        case BO.optionsCart.Empty:
                            bl.Cart.Empty(myCart);
                            break;
                        case BO.optionsCart.Approve:
                            Console.WriteLine(bl.Cart.Approve(myCart));
                            bl.Cart.Empty(myCart);
                            break;
                        case BO.optionsCart.Return:
                            break;
                        default:
                            throw new InvalidArgumentException("Not a valid option on menu.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Exception thrown from BO interface:\n{ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException.Message);
                }
                //re-ask for user input
            } while (op != BO.optionsCart.Return);
        }
        /// <summary>
        /// initializes a new produt
        /// </summary>
        /// <param name="updateId">optional- id</param>
        /// <returns></returns>
        /// <exception cref="InvalidArgumentException"></exception>
        static BO.Product InitializeProduct(int updateId = 0)
        {
            int id = updateId;
            //reading input:
            if (updateId == 0)
            {
                int.TryParse(Console.ReadLine(), out id);
            }
            string name = Console.ReadLine();
            double.TryParse(Console.ReadLine(), out double price);
            BO.category category;
            try
            {
                category = (BO.category)Enum.Parse(typeof(BO.category), Console.ReadLine());
            }
            catch (Exception ex){ throw new InvalidArgumentException(ex); }
            int.TryParse(Console.ReadLine(), out int inStock);
            //innitialize:
            return new BO.Product() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
        }
        /// <summary>
        /// creates cart
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidArgumentException"></exception>
        static BO.Cart InitializeCart()
        {
            if (myCart == null)
            {
                Console.WriteLine("Please login:\nEnter customer name, email, address");
                string name, email, adress;
                //reading input:
                try
                {
                    name = Console.ReadLine();
                    email = Console.ReadLine();
                    adress = Console.ReadLine();
                    //string attributes validation
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(adress) || !email.Contains('@') || email.StartsWith('@'))
                        throw new InvalidArgumentException("One or more attributes of the cart order are invalid.\n");
                }
                catch (Exception ex) { throw; }
                List<BO.OrderItem> Items = new List<BO.OrderItem>();
                myCart = new BO.Cart()
                {
                    CustomerName = name,
                    CustomerEmail = email,
                    CustomerAddress = adress,
                    Items = Items,
                    TotalPrice = 0,
                };
            }
            return myCart;
        }        
        /// <summary>
        /// initializes order item
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidArgumentException"></exception>
        static BO.OrderItem InitializeOrderItem()
        {
            //reading input:
            string name = Console.ReadLine();
            int.TryParse(Console.ReadLine(), out int prodId);
            double.TryParse(Console.ReadLine(), out double price);
            int.TryParse(Console.ReadLine(), out int amount);
            //input validation
            if (name.Length <= 0 && price < 0 && amount == 0)
                throw new InvalidArgumentException("One or more attributes of the initialization are invalid.\n");
            //innitialize order item
           return new BO.OrderItem()
            {
               Name = name,
               ProductId = prodId,
               Price = price,   
               Amount = amount,
               TotalPrice=amount*price
            };
        }

         //bool IsAllChar(this string s) { foreach (char c in s) if(!Char.IsLetter(c)) return false; return true; }
    }
}
