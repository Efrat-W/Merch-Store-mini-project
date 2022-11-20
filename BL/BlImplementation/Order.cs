using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Order : IOrder
{
    IDal dal = new DalList();
}
