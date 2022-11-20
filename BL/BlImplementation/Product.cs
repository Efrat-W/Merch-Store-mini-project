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
        DO.Product prod = new()
        {
            ID = product.ID,
            Name = product.Name,
            Price = product.Price,
            InStock = product.InStock,
            Category = (DO.category)product.Category
        };
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
        try
        {
            dal.Product.RequestById(id);
        }
        catch (Exception)
        { throw; }
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
        BO.Product newProd = new()
        {
            ID = prod.ID,
            Name = prod.Name,
            Category = (BO.category)prod.Category,
            Price = prod.Price
        };
        return newProd;
    }

    public IEnumerable<BO.ProductForList> RequestList()
    {
        IEnumerable<DO.Product> ls = dal.Product.RequestAll();
        IEnumerable<BO.ProductForList> productForLists = new List<BO.ProductForList>();
        foreach (DO.Product prod in ls)
        {
            ProductForList prodForLs = new()
            {
                ID = prod.ID,
                Name = prod.Name,
                Price = prod.Price,
                Category = (BO.category)prod.Category
            };

            productForLists.Append(prodForLs); //🤞
        }
        return productForLists;
    }

    public void Update(BO.Product product)
    {
        
    }
}
