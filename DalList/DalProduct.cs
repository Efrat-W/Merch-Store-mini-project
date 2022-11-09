
using DO;
using System.Collections.Generic;

namespace Dal;

public class DalProduct
{
    public void Create(Product prod)
    {
        if (DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("product already exists");
        DataSource.products.Add(prod);
    }

    public List<Product> RequestAll()
    {
        List <Product> productList = new List<Product>();
        foreach (Product product in DataSource.products)
            productList.Add(product);
        return productList; 
    }

    public Product RequestById(int id)
    {
        if (!DataSource.products.Exists(i => i.ID == id))
            throw new Exception("the product does not exist");
        return DataSource.products.Find(i => i.ID == id);
    }

    public void Update(Product prod)
    {
        //if order is not exist throw exception 
        if (!DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("cannot update, the order does not exists");
        Product prodToRemove = DataSource.products.Find(i => i.ID == prod.ID);
        DataSource.products.Remove(prodToRemove);
        DataSource.products.Add(prod);
    }

    public void Delete(Product prod)
    { 
        if (!DataSource.products.Exists(i => i.ID == prod.ID))
            throw new Exception("cannot delete, product does not exists");
        DataSource.products.Remove(prod); 
    }



}