
using DO;

namespace Dal;

internal static class DataSource
{
    readonly static Random rand = new Random();
    internal static List<Product?>? products = new();
    internal static List<Order?>? orders = new();
    internal static List<OrderItem?>? orderItems = new();
    static DataSource() => s_Initialize();
    private static void s_Initialize()
    {
        initProducts();
        initOrders();
        initOrderItems();
    }

    /// <summary>
    /// initializes the product list with 10 random products
    /// </summary>
    private static void initProducts()
    {
        Array categories = Enum.GetValues(typeof(category));
        int num = 100000;
        for (int i = 0; i < 10; i++)
        {
            Product prod = new Product() 
            {
                ID = num++,
                Price = (double)rand.Next(50*7, 200*7) /7,
                Name = "product " + i,
                Category = (category)rand.Next(categories.Length),
                InStock = (int)rand.Next(50)
            };
            products.Add(prod);
        }
    }

    /// <summary>
    /// initializes the orders list with 20 random orders
    /// </summary>
    private static void initOrders()
    {
        for (int i = 1; i <= 20; i++)
        {
            DateTime? randomShipDate = new();
            DateTime? randomDeliveryDate = new();
            DateTime? randomOrderDate = new();
            randomOrderDate = DateTime.Now;

            if (i < 0.8 * 20)
            {
                randomShipDate = randomOrderDate - new TimeSpan(rand.Next(7), rand.Next(23), rand.Next(59), 0);
                if (i < 0.6 * 20)
                    randomDeliveryDate = randomShipDate - new TimeSpan(rand.Next(7), rand.Next(23), rand.Next(59), 0);
                else
                    randomDeliveryDate = null;
            }
            else
                randomShipDate = randomDeliveryDate = null;

            Order order = new Order()
            {
                ID=Config.OrderSeqID,
                CustomerAddress = $"Rabbi Akiva {i}, Bnei Brak",
                CustomerEmail = $"user{i}@gmail.com",
                CustomerName = "customer no. " + i,
                OrderDate = randomOrderDate,
                ShipDate = randomShipDate,
                DeliveryDate = randomDeliveryDate
            };
            orders.Add(order);
        }
    }

    /// <summary>
    /// initializes the order items list with 20-80 random items
    /// </summary>
    private static void initOrderItems()
    {

        foreach (Order order in orders)
        {
            int numOfProducts = rand.Next(1, 4);
            for (int i = 0; i < numOfProducts; i++)
            {
                Product? prod = products[rand.Next(products.Count)];

                OrderItem item = new OrderItem()
                {
                    ID = Config.OrderItemSeqID,
                    OrderID = order.ID,
                    ProductID = (int)prod?.ID,
                    Price = (double)prod?.Price,
                    Amount = rand.Next(1, (int)prod?.InStock)
                };
                orderItems.Add(item);
            }
        }
    }

    /// <summary>
    /// class for sequence numbers
    /// </summary>
    internal static class Config
    {
        private static int orderItemSeqID = 1;
        public static int OrderItemSeqID => orderItemSeqID++;

        private static int orderSeqID = 1;
        public static int OrderSeqID => orderSeqID++;
    }
}
