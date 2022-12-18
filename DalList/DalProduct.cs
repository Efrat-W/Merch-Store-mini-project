
using DalApi;
using DO;
using System.Collections.Generic;

namespace Dal;

internal class DalProduct : IProduct
{
    /// <summary>
    /// adds the product to the list of products
    /// </summary>
    /// <param name="prod">the new product</param>
    /// <exception cref="Exception"></exception>
    public int Create(Product prod)
    {
            Product? prodCheck = DataSource.products.Find(i => i?.ID == prod.ID);
            if (prodCheck != null)
                throw new MissingEntityException("Requested Product already exists.\n");
            DataSource.products.Add(prod);
            return prod.ID;
        
    }

    /// <summary>
    /// returns the list of products
    /// </summary>
    /// <returns><list type="Product">list of products</returns>
    public IEnumerable<Product?> RequestAll(Func<Product?, bool>? func = null)
    {
        if (func == null)
            return DataSource.products.Select(o => o);
        return DataSource.products.Where(o => func(o));
    }

    /// <summary>
    /// returns the product by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Product RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }

    /// <summary>
    /// returns the product by given func condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Product RequestByFunc(Func<Product?, bool>? func)
    {
        return DataSource.products.Find(p=> func(p)) ?? throw new MissingEntityException("Requested Product does not exist.\n");
    }


    /// <summary>
    ///  updates the order with the same id to the given order's data
    /// </summary>
    /// <param name="prod">the updated product</param>
    /// <exception cref="Exception"></exception>
    public void Update(Product prod)
    {
        lock (this)
        {
            //if product is not exist throw exception 
            Product? prodToRemove = DataSource.products.Find(i => i?.ID == prod.ID) ?? throw new MissingEntityException("Requested Product does not exist.\n");
            DataSource.products.Remove(prodToRemove);
            DataSource.products.Add(prod);
        }
    }

    /// <summary>
    /// deletes the product from the list 
    /// </summary>
    /// <param name="prod"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(Product prod)
    {
        lock (this)
        {
            Product? prodToRemove = DataSource.products.Find(i => i?.ID == prod.ID) ?? throw new MissingEntityException("Requested Product does not exist.\n");
            DataSource.products.Remove(prodToRemove);
        }
    }
}