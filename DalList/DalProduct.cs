
using DO;
using System.Collections.Generic;

namespace Dal;

public class DalProduct
{
    /// <summary>
    /// adds the product to the list of products
    /// </summary>
    /// <param name="prod">the new product</param>
    /// <exception cref="Exception"></exception>
    public void Create(Product prod)
    {
        if (DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("product already exists");
        DataSource.products.Add(prod);
    }

    /// <summary>
    /// returns the list of products
    /// </summary>
    /// <returns><list type="Product">list of products</returns>
    public List<Product> RequestAll()
    {
        List <Product> productList = new List<Product>();
        foreach (Product product in DataSource.products)
            productList.Add(product);
        return productList; 
    }

    /// <summary>
    /// returns the product by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Product RequestById(int id)
    {
        if (!DataSource.products.Exists(i => i.ID == id))
            throw new Exception("the product does not exist");
        return DataSource.products.Find(i => i.ID == id);
    }

    /// <summary>
    ///  updates the order with the same id to the given order's data
    /// </summary>
    /// <param name="prod">the updated product</param>
    /// <exception cref="Exception"></exception>
    public void Update(Product prod)
    {
        //if order is not exist throw exception 
        if (!DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("cannot update, the order does not exists");
        Product prodToRemove = DataSource.products.Find(i => i.ID == prod.ID);
        DataSource.products.Remove(prodToRemove);
        DataSource.products.Add(prod);
    }

    /// <summary>
    /// deletes the product from the list 
    /// </summary>
    /// <param name="prod"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(Product prod)
    { 
        if (!DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("cannot delete, product does not exists");
        DataSource.products.Remove(prod); 
    }



}