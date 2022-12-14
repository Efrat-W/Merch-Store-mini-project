
using DalApi;

namespace Dal;

sealed public class DalList : IDal
{
    public static readonly Lazy<DalList> lazy = new Lazy<DalList>(() => new DalList());
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
