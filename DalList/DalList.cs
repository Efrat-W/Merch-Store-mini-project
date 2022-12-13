
using DalApi;

namespace Dal;

sealed public class DalList : IDal
{
    public static DalList Instance { get; } = new DalList();
    private DalList() { }
    public IProduct Product => new DalProduct();
    public IOrder Order => new DalOrder();
    public IOrderItem OrderItem => new DalOrderItem();
}
