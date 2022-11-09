
using DO;

namespace Dal;

internal static class DataSource
{
    readonly static Random rand = new Random();
    internal static List<Product> products = new List<Product>();
    internal static List<Order> orders = new List<Order>();
    internal static List<OrderItem> orderItems = new List<OrderItem>();

    private static void s_Initialize()
    {
        initProducts();
        initOrders();
        initOrderItems();
    }

    private static void initProducts()
    {
        Array categories = Enum.GetValues(typeof(category));
        DalProduct dalProduct = new DalProduct();
        int num = 100000;
        for (int i = 0; i < 10; i++)
        {
            Product prod = new Product()
            {
                ID = num++,
                Price = rand.Next(50, 200),
                Name = "product" + i,
                Category = (category)categories.GetValue(rand.Next(categories.Length)),
                InStock = rand.Next(50)
            };
            dalProduct.Create(prod);
        }
    }

    private static void initOrders()
    {
        DalOrder dalOrder = new DalOrder();

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
            dalOrder.Create(order);
        }
    }

    private static void initOrderItems()
    {
        DalOrderItem dalOrderItem = new DalOrderItem();
        DalProduct dalProduct = new DalProduct();

        foreach (Order order in orders)
        {
            int numOfProducts = rand.Next(1, 4);
            for (int i = 0; i < numOfProducts; i++)
            {
                Product prod = dalProduct.RequestById(rand.Next(20) + 100000);

                OrderItem item = new OrderItem()
                {
                    OrderID = order.ID,
                    ProductID = prod.ID,
                    Price = prod.Price,
                    Amount = rand.Next(1, prod.InStock)
                };
                dalOrderItem.Create(item);
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
