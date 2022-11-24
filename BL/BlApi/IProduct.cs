
using BO;
namespace BlApi;

public interface IProduct
{
    public IEnumerable<ProductForList> RequestList();

    public Product RequestByIdManager(int id);
    public Product RequestByIdCustomer(int id, Cart cart);
    public Product Add(Product product);
    public Product Update(Product product);
    public void Delete(int id);
}