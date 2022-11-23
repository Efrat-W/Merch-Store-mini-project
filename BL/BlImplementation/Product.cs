using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using BO;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Product : IProduct
{
    IDal dal = new DalList();
    internal bool CheckProduct(BO.Product product) {
        if (product.ID > 0 && product.Name != "" && product.Price > 0 && product.InStock >= 0)
            return true;
        return false;
    }
    public void Add(BO.Product product)
    {
        if (!CheckProduct(product))
            throw;
        DO.Product prod = product.ProductBoToDo();
        try
        {
            dal.Product.Create(prod);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void Delete(int id)
    {
        IEnumerable<DO.OrderItem> orderItems = dal.OrderItem.RequestAll();

        IEnumerable<DO.OrderItem> found = from orderItem in orderItems
                                          where orderItem.ProductID == id
                                          select orderItem;
        //try exception missing
        if (found.Count<DO.OrderItem>() == 0)
        {
            dal.Product.Delete(dal.Product.RequestById(id));
        }
    }

    public BO.Product RequestById(int id)
    {
        DO.Product prod;
        if (id < 0)
            throw new Exception("invalid id");
        else
        {
            try
            {
                prod = dal.Product.RequestById(id);
            }
            catch { throw; }
        }
        return prod.ProductDoToBo();
    }

    public IEnumerable<BO.ProductForList> RequestList() {
        return from doProd in dal.Product.RequestAll()
               select new BO.ProductForList()
               {
                   ID = doProd.ID,
                   Name = doProd.Name,
                   Price = doProd.Price,
                   Category = (BO.category)doProd.Category
               };
    }

    public void Update(BO.Product product)
    {
        if (product.ID > 99999 && product.ID <= 999999  //product id has 6 digits
            && product.Name.Length > 0 && product.InStock >= 0) 
        {
            dal.Product.Update(product.ProductBoToDo());
        }
    }
}


// Tool functions
static class ProductTools
{
    public static DO.Product ProductBoToDo(this BO.Product prod)
    {
        return new DO.Product()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (DO.category)prod.Category,
            Price = prod.Price
        };
    }

    public static BO.Product ProductDoToBo(this DO.Product prod)
    {
        return new BO.Product()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (BO.category)prod.Category,
            Price = prod.Price
        };
    }
}
