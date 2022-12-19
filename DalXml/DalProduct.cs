namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

internal class DalProduct : IProduct
{
    string path = "products.xml";
    string configPath = "config.xml";
    XElement productsRoot;
    public DalProduct()
    {
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(path))
                productsRoot = XElement.Load(@"..\xml\" + path);
            else
            {
                productsRoot = new XElement("products");
                productsRoot.Save(path);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("product File upload problem" + ex.Message);
        }
    }
    public int Create(Product prod)
    { 
        XElement Id = new ("ID", prod.ID);
        XElement Name = new ("Name", prod.Name);
        XElement Price = new ("Price", prod.Price);
        XElement Category = new ("Category", prod.Category);
        XElement InStock = new ("InStock", prod.InStock);

        productsRoot.Add(new XElement("product", Id, Name, Price, Category, InStock));
        productsRoot.Save(path);

        return prod.ID;
    }

    public IEnumerable<Product?> RequestAll(Func<Product?, bool>? func = null)
    {
        //LoadData();
        IEnumerable<Product?> products;
        try
        {
            products = (from p in productsRoot.Elements()
                                              let prod= new Product()
                                              {
                                                  ID = int.Parse(p.Element("ID").Value),
                                                  Name = p.Element("Name").Value,
                                                  Price = double.Parse(p.Element("Price").Value),
                                                  InStock = int.Parse(p.Element("InStock").Value),
                                                  Category = (category)Enum.Parse(typeof(category), p.Element("Category").Value)
                                              }
                        select (Product?)prod);
        }
        catch (Exception ex)
        {
            products = null;
        }
        if (func == null)
         return products;
        return products!.Where(o => func(o));
        
    }

    public Product RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }
    //logically right???
    public Product RequestByFunc(Func<Product?, bool>? func)
    {
        IEnumerable<Product?> filteredProducts= RequestAll(func) ?? throw new MissingEntityException("Requested Product does not exist.\n");
        return filteredProducts.First() ?? throw new MissingEntityException("Requested Product does not exist.\n"); ;
    }
    public void Update(Product prod)
    {
        try
        {
            Delete(prod);
            Create(prod);
        }
        catch
        {
            throw;
        }
    }
    public void Delete(Product prod)
    {
        XElement productElement;
        try
        {
            productElement = (XElement)(from p in productsRoot.Elements()
                                        where int.Parse(p.Element("ID").Value) == prod.ID
                              select p).FirstOrDefault();
            productElement.Remove();
            productsRoot.Save(path);
       }
        catch
        {
            throw;
        }
    }
}
