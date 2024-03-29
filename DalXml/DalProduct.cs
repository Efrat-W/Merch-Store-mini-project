﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
internal class DalProduct : IProduct
{
    string path = "../xml/products.xml";
    XElement productsRoot;
    [MethodImpl(MethodImplOptions.Synchronized)]
    public DalProduct()
    {
        LoadData();
    }
    /// <summary>
    /// loadind the file's data to the root
    /// </summary>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private void LoadData()
    {
        try
        {
            if (File.Exists(path))
                productsRoot = XElement.Load(path);
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
    /// <summary>
    /// adds the product to the list of products
    /// </summary>
    /// <param name="prod">the new product</param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Product prod)
    {
        XElement Id = new("ID", prod.ID);
        XElement Name = new("Name", prod.Name);
        XElement Price = new("Price", prod.Price);
        XElement Category = new("Category", prod.Category);
        XElement InStock = new("InStock", prod.InStock);
        XElement Image = new("Image", prod.Image);
        XElement Description = new("Description", prod.Description);

        productsRoot.Add(new XElement("product", Id, Name, Price, Category, InStock, Image, Description));
        productsRoot.Save(path);

        return prod.ID;
    }
    /// <summary>
    /// returns the list of products
    /// </summary>
    /// <returns><list type="Product">list of products</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Product?> RequestAll(Func<Product?, bool>? func = null)
    {
        IEnumerable<Product?> products;
        try
        {
            products = (from p in productsRoot.Elements()
                        let prod = new Product()
                        {
                            ID = int.Parse(p.Element("ID")!.Value),
                            Name = p.Element("Name")!.Value,
                            Price = double.Parse(p.Element("Price")!.Value),
                            InStock = int.Parse(p.Element("InStock")!.Value),
                            Category = (category)Enum.Parse(typeof(category), p.Element("Category")!.Value),
                            Image = p.Element("Image")!.Value,
                            Description = p.Element("Description")!.Value
                        }
                        select (Product?)prod);
        }
        catch (Exception ex)
        {
            products = null;
        }
        if (func == null)
            return products;
        //if there is any condition
        return products!.Where(o => func(o));

    }
    /// <summary>
    /// returns the product by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Product RequestById(int id)
    {
        return RequestByFunc(i => i?.ID == id);
    }
    /// <summary>
    /// returns the product by given func condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Product RequestByFunc(Func<Product?, bool>? func)
    {
        IEnumerable<Product?> filteredProducts = RequestAll(func) ?? throw new MissingEntityException("Requested Product does not exist.\n");
        return filteredProducts.First() ?? throw new MissingEntityException("Requested Product does not exist.\n"); ;
    }
    /// <summary>
    ///  updates the order with the same id to the given order's data
    /// </summary>
    /// <param name="prod">the updated product</param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Product prod)
    {
        try
        {
            Delete(prod);//deletes the old object
            Create(prod);//creates the new one
        }
        catch (Exception e)
        {
            throw new MissingEntityException("There is no such product..");
        }
    }
    /// <summary>
    /// deletes the product from the list
    /// </summary>
    /// <param name="prod"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(Product prod)
    {
        XElement productElement;
        try
        {
            productElement = (from p in productsRoot.Elements()
                              where int.Parse(p.Element("ID")!.Value) == prod.ID
                              select p).FirstOrDefault()!;
            productElement.Remove();
            productsRoot.Save(path);
        }
        catch (Exception e)
        {
            throw new MissingEntityException("There is no such product..");
        }
    }
}