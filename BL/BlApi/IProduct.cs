
using BO;
namespace BlApi;

public interface IProduct
{
    public IEnumerable<Product> RequestList();

    public Product RequestById(int id);
    public void Add(Product product);
    public void Update(Product product);
    public void Delete(int id);
}