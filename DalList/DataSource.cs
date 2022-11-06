
using DO;

namespace Dal;

internal static class DataSource
{
    readonly static Random RandomNum = new Random();
    static Product[] products = new Product[50];
    static Order[] orders = new Order[100];
    static OrderItem[] orderItems = new OrderItem[200];
}
