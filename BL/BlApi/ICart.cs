
using BO;
namespace BlApi;

public interface ICart
{
    public void Add(Cart cart, int prodId);

    public void Update(Cart cart, int prodId, int amount);

    public void Approve(Cart cart, string name, string email, string address);
}
