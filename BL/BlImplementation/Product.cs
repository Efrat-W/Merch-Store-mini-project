using BO;


namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    DalApi.IDal? dal = DalApi.Factory.Get();
    /// <summary>
    /// help method for veryfying a new product 
    /// </summary>
    /// <param name="product"></param>
    /// <exception cref="InvalidArgumentException"></exception>
    private void CheckProduct(BO.Product product)
    {
        if (!(product.ID > 99999 && product.Name!.Length > 0 && product.Price > 0 && product.InStock >= 0))
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
            prod = product.ProductBoToDo();//converts to DO
        }
        catch { throw; }
        try
        {
            dal!.Product.Create(prod);//tries to add to DO
        }
        catch (DO.DoubledEntityException ex)
        {
            throw new InvalidArgumentException("Requested Product to create wasn't found.", ex);
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
        IEnumerable<DO.OrderItem?> orderItems = dal!.OrderItem.RequestAll();
        IEnumerable<DO.OrderItem?> found = from orderItem in orderItems //checks if the product exist in any order
                                           where orderItem?.ProductID == id
                                           select orderItem;
        if (found.Count() == 0)//no items from this product where ordered
        {
            try
            {
                dal.Product.Delete(dal.Product.RequestById(id));
            }
            catch (DO.MissingEntityException ex)
            {
                throw new InvalidArgumentException("Requested Product to delete couldn't be found.", ex);
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
            throw new InvalidArgumentException("Invalid id input.\n");
        else
        {
            try
            {
                prod = dal!.Product.RequestById(id);
            }
            catch (Exception ex) { throw new InvalidArgumentException("Requested Product isn't found.", ex); }
        }
        return prod.ProductDoToBo();//converts to BO
    }
    /// <summary>
    /// returns a BO product item entity by product id for customer
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cart">the customer's cart</param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    /// <exception cref="EntityNotFoundException"></exception>
    public ProductItem RequestByIdCustomer(int id, BO.Cart cart)
    {
        DO.Product? prod;
        if (id < 0)
            throw new InvalidArgumentException("Invalid id input.\n");
        else
        {
            try
            {
                prod = dal!.Product.RequestById(id);
            }
            catch (Exception ex) { throw new EntityNotFoundException("Requested Product isn't found.", ex); }
        }
        BO.OrderItem? item = null;
        if (cart.Items != null)
        {
            try
            {
                item = cart.Items.Find(i => i!.ProductId == id);//checks if the item exists in cart
            }
            catch (DO.MissingEntityException) { throw new EntityNotFoundException("your cart does not contain this product"); }
        }

        int amount = 0;
        if (item != null)
            amount = item.Amount;
        return new BO.ProductItem()
        {
            ID = prod?.ID ?? throw new InvalidArgumentException(),
            Name = prod?.Name,
            Category = (BO.category)prod?.Category!,
            Price = prod?.Price ?? throw new InvalidArgumentException(),
            Amount = amount,
            InStock = prod?.InStock > amount,
            Image=prod?.Image,
            Description=prod?.Description,
        };
    }
    /// <summary>
    /// returns the catalog (list of productForList items)
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.ProductForList> RequestList()
    {
        return from doProd in dal!.Product.RequestAll()
               select new BO.ProductForList()//converts from DO to BO and returns list
               {
                   ID = doProd?.ID ?? throw new InvalidArgumentException(),
                   Name = doProd?.Name,
                   Price = doProd?.Price ?? throw new InvalidArgumentException(),
                   Category = (BO.category)doProd?.Category!,
                   Image = doProd?.Image,
                   Description = doProd?.Description
               };
    }

    /// <summary>
    /// returns a catalog (list of productforlist items) by given condition
    /// </summary>
    /// <param name="func">condition</param>
    /// <returns></returns>
    public IEnumerable<ProductForList> RequestListByCond(Func<ProductForList, bool>? func = null)
    {
        if (func == null)
            return RequestList();
        return RequestList().Where(func!);
    }


    /// <summary>
    /// updates a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Product Update(BO.Product product)
    {
        //validation
        if (product.ID > 99999 && product.Name!.Length > 0 && product.InStock >= 0 && product.Price >= 0)
        {
            try
            {
                dal!.Product.Update(product.ProductBoToDo());
            }
            catch (DO.MissingEntityException ex) { throw new InvalidArgumentException("Requested Product to update isn't found.", ex); }
        }
        else
        {
            throw new InvalidArgumentException("id out of range.\n");
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
            Category = (DO.category)prod.Category!,
            Price = prod.Price,
            InStock = prod.InStock,
            Image=prod.Image!,
            Description=prod!.Description!
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
            Category = (BO.category)prod.Category!,
            Price = prod.Price,
            InStock = prod.InStock,
            Image = prod.Image,
            Description = prod.Description!
        };
    }
}
