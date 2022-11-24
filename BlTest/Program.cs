using BlApi;
using BlImplementation;
using Dal;
using DO;

namespace BlTest
{
    internal class Program
    {
        static private IBl bl = new Bl();
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
                        program.CartMenu();
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
            do
            {
                switch (op)
                {
                    case options.Add:
                        Console.WriteLine("Enter ID, Name, Price, category and the amount in stock");
                        bl.Product.Add(InitializeProduct());
                        break;
                    case options.ShowById:
                        Console.WriteLine("Enter ID");
                        int.TryParse(Console.ReadLine(), out int id);
                        Console.WriteLine(bl.Product.RequestById(id)); //print requested product to console
                        break;
                    case options.ShowList:
                        IEnumerable<Product> productList = bl.Product.RequestAll();
                        foreach (Product prod in productList)
                            Console.WriteLine(prod); ;
                        break;
                    case options.Update:
                        Console.WriteLine("Enter the existing product's ID");
                        id = int.Parse(Console.ReadLine());
                        Product update = bl.Product.RequestById(id);
                        Console.WriteLine(update);
                        Console.WriteLine("Enter the new Name, Price, category and the amount in stock");
                        bl.Product.Update(InitializeProduct(id));
                        break;
                    case options.DeleteFromList:
                        Console.WriteLine("Enter the ID of the product you wish to remove ");
                        Product product = new Product() { ID = int.Parse(Console.ReadLine()) };
                        bl.Product.Delete(product);
                        break;
                    default:
                        break;
                }
                //re-ask for user input
                input = Console.ReadLine();
                op = (options)Enum.Parse(typeof(options), input);
            } while (op != options.Return);
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
            BO.category category = (BO.category)Enum.Parse(typeof(BO.category), Console.ReadLine());
            input = Console.ReadLine();
            int.TryParse(input, out int inStock);

            BO.Product product = new BO.Product() { ID = id, Category = category, InStock = inStock, Name = name, Price = price };
            return product;
        }
    }
}
