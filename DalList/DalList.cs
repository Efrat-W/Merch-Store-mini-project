
using DalApi;

namespace Dal;
//lazy and thread safe singleton for saving the data in lists 
sealed internal class DalList : IDal
{
    private static readonly Lazy<DalList> lazy = new (() => new DalList());
    public static DalList Instance { get { return lazy.Value; } }
    // private constructor 
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
