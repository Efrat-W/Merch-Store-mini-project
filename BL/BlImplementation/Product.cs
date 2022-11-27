using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using BO;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    IDal dal = new DalList();
    //auxillary method
    private void CheckProduct(BO.Product product) {
        if (!(product.ID > 0 && product.Name != "" && product.Price > 0 && product.InStock >= 0))
            throw new InvalidArgumentException("one or more attributes of product are invalid. \n");
    }
    public BO.Product Add(BO.Product product)
    {
        DO.Product prod;
        try
        {
            CheckProduct(product);

            prod = product.ProductBoToDo();
        }
        catch { throw; }
        try
        {
            dal.Product.Create(prod);
        }
        catch (DoubledEntityException ex)
        {
            throw new InvalidArgumentException(ex);
        }
        return product;
    }

    public void Delete(int id)
    {
        IEnumerable<DO.OrderItem> orderItems = dal.OrderItem.RequestAll();

        IEnumerable<DO.OrderItem> found = from orderItem in orderItems
                                          where orderItem.ProductID == id
                                          select orderItem;
        if (found.Count() == 0)
        {
            dal.Product.Delete(dal.Product.RequestById(id));
        }
        else
            throw new InvalidArgumentException("cannot delete product that is actively bought.\n");
    }

    public BO.Product RequestByIdManager(int id)
    {
        DO.Product prod;
        if (id < 0)
            throw new InvalidArgumentException();
        else
        {
            try
            {
                prod = dal.Product.RequestById(id);
            }
            catch(MissingEntityException ex) { throw new InvalidArgumentException(ex); }
        }
        return prod.ProductDoToBo();
    }

    public BO.ProductItem RequestByIdCustomer(int id, BO.Cart cart)
    {
        DO.Product prod;
        if (id < 0)
            throw new InvalidArgumentException();
        else
        {
            try
            {
                prod = dal.Product.RequestById(id);
            }
            catch { throw new EntityNotFoundException(); }
        }
        BO.OrderItem item = cart.Items.Find(i => i.ID == id);
        return new BO.ProductItem()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (BO.category)prod.Category,
            Price = prod.Price,
            Amount = item.Amount,
            InStock = item.Amount <= prod.InStock
        };
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

    public BO.Product Update(BO.Product product)
    {
        if (product.ID > 99999 && product.ID <= 999999  //product id has 6 digits
            && product.Name.Length > 0 && product.InStock >= 0) 
        {
            dal.Product.Update(product.ProductBoToDo());
        }
        else
        {
            throw new InvalidArgumentException();
        }
        return product;
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
            Price = prod.Price,
            InStock = prod.InStock,
        };
    }

    public static BO.ProductItem ProductItemDoToBo(this DO.Product prod)
    {
        return new BO.ProductItem()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (BO.category)prod.Category,
            Price = prod.Price,
            InStock = prod.InStock > 0
        };
    }

    public static BO.Product ProductDoToBo(this DO.Product prod)
    {
        return new BO.Product()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (BO.category)prod.Category,
            Price = prod.Price,
            InStock = prod.InStock
        };
    }
}
