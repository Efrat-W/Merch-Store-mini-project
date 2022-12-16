
using DalApi;

namespace Dal;

sealed internal class DalList : IDal
{
    private static readonly Lazy<DalList> lazy = new (() => new DalList());
    public static DalList Instance { get { return lazy.Value; } }
    private DalList()
    { 
        Product = new DalProduct();
        Order = new DalOrder();
        OrderItem = new DalOrderItem();
    }
    public IProduct Product { get; }
    public IOrder Order { get; }
    public IOrderItem OrderItem { get; }
}
