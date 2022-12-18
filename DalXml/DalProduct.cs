namespace Dal;
using DalApi;
using DO;
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
                productsRoot = XElement.Load(@"xml/" + path);
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
        XElement Id = new XElement("Id", prod.ID);
        XElement Name = new XElement("Name", prod.Name);
        XElement Price = new XElement("Price", prod.Price);
        XElement Category = new XElement("Category", prod.Category);
        XElement InStock = new XElement("In Stock", prod.InStock);

        productsRoot.Add(new XElement("product", Id, Name, Price, Category, InStock));
        productsRoot.Save(path);

        return prod.ID;
    }

    public IEnumerable<Product?> RequestAll(Func<Product?, bool>? func = null)
    {
        LoadData();
        IEnumerable<Product?> products;
        try
        {
            products = (from p in productsRoot.Elements()
                        let prod = new Product()
                        {
                            ID = Convert.ToInt32(p.Element("Id")!.Value)                                              ,
                            Name = p.Element("Name")!.Value,
                            Price = Convert.ToInt32(p.Element("Price")!.Value),
                            InStock = Convert.ToInt32(p.Element("In Stock")!.Value),
                            Category = (category)Enum.Parse(typeof(category), p.Element("Category")!.Value)
                        }
                        select (Product?)prod);
        }
        catch (Exception ex)
        {
            products = null;
        }
        if (func == null)
            return products;
        return DataSource.products.Where(o => func(o));
    }
}
