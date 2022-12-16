using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

sealed internal class DalXml: IDal
{
    //lazy and thread safe singleton for saving the data in lists 
    
        private static readonly Lazy<DalXml> lazy = new(() => new DalXml());
        public static DalXml Instance { get { return lazy.Value; } }
        private DalXml()
        {
            Product = new Dal.Product();
            Order = new Dal.Order();
            OrderItem = new Dal.OrderItem();
        }
        public IProduct Product { get; }
        public IOrder Order { get; }
        public IOrderItem OrderItem { get; }
    
}
