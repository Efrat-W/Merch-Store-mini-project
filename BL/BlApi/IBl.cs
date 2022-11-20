
using DalApi;

namespace BlApi;

public interface IBl
{
    IProduct Product { get; }
    IOrder Order { get; }
    ICart Cart { get; }
}
