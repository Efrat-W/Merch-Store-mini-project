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
    /// <summary>
    /// help method for veryfying a new product 
    /// </summary>
    /// <param name="product"></param>
    /// <exception cref="InvalidArgumentException"></exception>
    private void CheckProduct(BO.Product product) {
        if (!(product.ID > 0 && product.Name != "" && product.Price > 0 && product.InStock >= 0))
            throw new InvalidArgumentException("one or more attributes of product are invalid. \n");
    }
    /// <summary>
    /// adds a new product to the catalog
    /// </summary>
    /// <param name="product"></param>
    /// <returns>the new product</returns>
    /// <exception cref="InvalidArgumentException"></exception>
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
    /// <summary>
    /// deletes a product from the catalog (only if it
    /// doesnt exist in any order)
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="InvalidArgumentException"></exception>
    public void Delete(int id)
    {
        IEnumerable<DO.OrderItem> orderItems = dal.OrderItem.RequestAll();
        IEnumerable<DO.OrderItem> found = from orderItem in orderItems
                                          where orderItem.ProductID == id
                                          select orderItem;
        if (found.Count() == 0)//no items from this product where ordered
        {
            try
            {
                dal.Product.Delete(dal.Product.RequestById(id));
            }
            catch(MissingEntityException ex) 
            {
                throw new InvalidArgumentException(ex); 
            }
        }
        else
            throw new InvalidArgumentException("cannot delete product that is actively bought.\n");
    }
    /// <summary>
    /// returns a BO product entity by product id for manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
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
        return prod.ProductDoToBo();//converts
    }
    /// <summary>
    /// returns a BO product item entity by product id for customer
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cart">the customer's cart</param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    /// <exception cref="EntityNotFoundException"></exception>
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
        BO.OrderItem item;
        try
        {
             item = cart.Items.Find(i => i.ID == id);
        }
        catch (MissingEntityException) { throw new EntityNotFoundException("your cart does not contain this product"); }
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
    /// <summary>
    /// returns the catalog (list of productForList items)
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// updates a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Product Update(BO.Product product)
    {
        if (product.ID > 99999 && product.ID <= 999999  //product id has 6 digits
            && product.Name.Length > 0 && product.InStock >= 0) 
        {
            try
            {
                dal.Product.Update(product.ProductBoToDo());
            }
            catch (MissingEntityException ex) { throw new InvalidArgumentException(ex); }
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
    /// <summary>
    /// converts a product from BO to DO
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
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
    /// <summary>
    /// converts a product from DO to BO
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
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
