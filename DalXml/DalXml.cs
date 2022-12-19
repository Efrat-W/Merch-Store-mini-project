using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal;

sealed internal class DalXml: IDal
{
    //lazy and thread safe singleton for saving the data in lists 
    
    private static readonly Lazy<DalXml> lazy = new(() => new DalXml());
    public static DalXml Instance { get { return lazy.Value; } }
    private DalXml() { }

    public IProduct Product { get; } = new DalProduct();
    public IOrder Order { get; } = new DalOrder();
    public IOrderItem OrderItem { get; } = new DalOrderItem();

}
