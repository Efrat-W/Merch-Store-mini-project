
using BO;
namespace BlApi;
//Product interface
public interface IProduct
{
    public IEnumerable<ProductForList> RequestList();
    public IEnumerable<ProductForList> RequestListByCond(Func<ProductForList, bool>? func = null);
    public Product RequestByIdManager(int id);
    public ProductItem RequestByIdCustomer(int id, Cart cart);
    public Product Add(Product product);
    public Product Update(Product product);
    public void Delete(int id);
}