
using DO;

namespace Dal;

internal static class DataSource
{
    readonly static Random rand = new Random();
    internal static List<Product> products = new List<Product>();
    internal static List<Order> orders = new List<Order>();
    internal static List<OrderItem> orderItems = new List<OrderItem>();

    static DataSource() => s_Initialize();
    private static void s_Initialize()
    {
        initProducts();
        initOrders();
        initOrderItems();
    }

    private static void initProducts()
    {
        Array categories = Enum.GetValues(typeof(category));
        int num = 100000;
        for (int i = 0; i < 10; i++)
        {
            Product prod = new Product();
            /*{
                ID = num++,
                Price = (double)rand.Next(50, 200),
                Name = "product " + i,
                //Category = (category)categories.GetValue(rand.Next(categories.Length)),
                InStock = (int)rand.Next(50)
            };*/
            
            prod.Price = (double)rand.Next(50*7,200*7) / 7;
            prod.Name = "product " + i;
            prod.ID = num++;
            prod.Category = (category)rand.Next(0, 5);
            prod.InStock = rand.Next(50);
            products.Add(prod);
        }
    }

    private static void initOrders()
    {
        for (int i = 0; i < 20; i++)
        {
            DateTime randomShipDate = new DateTime();
            DateTime randomDeliveryDate = new DateTime();
            if (i < 0.8 * 20)
            { 
                randomShipDate = DateTime.MinValue + TimeSpan.FromDays((double)rand.Next(5));
                if (i < 0.6 * 20)
                    randomDeliveryDate = randomShipDate + TimeSpan.FromDays((double)rand.Next(7));
            }

            Order order = new Order()
            {
                CustomerAddress = $"Rabbi Akiva {i}, Bnei Brak",
                CustomerEmail = $"user{i}@gmail.com",
                CustomerName = "customer no. " + i,
                OrderDate = DateTime.MinValue,
                ShipDate = randomShipDate,
                DeliveryDate = randomDeliveryDate
            };
            orders.Add(order);
        }
    }

    private static void initOrderItems()
    {

        foreach (Order order in orders)
        {
            int numOfProducts = rand.Next(1, 4);
            for (int i = 0; i < numOfProducts; i++)
            {
                Product prod = products[rand.Next(1, 20)];

                OrderItem item = new OrderItem()
                {
                    OrderID = order.ID,
                    ProductID = prod.ID,
                    Price = prod.Price,
                    Amount = rand.Next(1, prod.InStock)
                };
                orderItems.Add(item);
            }
        }
    }

    internal static class Config
    {
        private static int orderItemSeqID = 0;
        public static int OrderItemSeqID => orderItemSeqID++;

        private static int orderSeqID = 0;
        public static int OrderSeqID => orderSeqID++;



    }
}
