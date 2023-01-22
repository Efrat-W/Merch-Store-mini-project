
using BO;
namespace BlApi;
//Cart interface
public interface ICart
{
    public Cart AddProduct(Cart cart, int prodId);

    public Cart UpdateProductAmount(Cart cart, int prodId, int amount);

    public BO.Order Approve(Cart cart);

    public void Empty(Cart cart);
}
