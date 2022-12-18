using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

using BO;
namespace BlImplementation;

internal class Bl : IBl
{
    public IProduct Product { get; } = new Product();
    public IOrder Order { get; } = new Order();
    public ICart Cart { get; } = new Cart();
}
