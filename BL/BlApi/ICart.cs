
using BO;
namespace BlApi;

public interface ICart
{
    public Cart AddProduct(Cart cart, int prodId);

    public Cart UpdateProductAmount(Cart cart, int prodId, int amount);

    public void Approve(Cart cart, string name, string email, string address);
}
