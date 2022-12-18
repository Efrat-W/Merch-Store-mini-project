using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using BO;


namespace BlImplementation;

internal class Cart : ICart
{
    DalApi.IDal? dal = DalApi.Factory.Get();
    /// <summary>
    /// adds a product to the customer's cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="prodId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Cart AddProduct(BO.Cart cart, int prodId)
    {
        DO.Product? prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (Exception ex)
        {
            if (prodId > 999999 || prodId <= 99999)
                throw new InvalidArgumentException("id out of range.\n");
            throw new EntityNotFoundException("Requested Product to add not found.", ex);
        }
        if (cart.Items!.FirstOrDefault(i => i!.ProductId == prodId) != null) //product exists in items
        {
            OrderItem item = cart.Items!.Find(i => i!.ProductId == prodId);
            if (prod?.InStock >= item!.Amount)//there is enough in stock
            {
                item.Amount++;
                item.TotalPrice += prod?.Price ?? throw new InvalidArgumentException();
                cart.TotalPrice += prod?.Price ?? throw new InvalidArgumentException();
            }
            else
                throw new InvalidArgumentException("The amount requested is greater than available.\n");
        }
        else//product does not exist in cart
        {
            if (prod?.InStock > 1)//adds product to cart
            {
                cart.Items!.Add(new BO.OrderItem 
                    { ID = 0, 
                        Amount = 1, 
                        Name = prod?.Name, 
                        Price = prod?.Price ?? throw new InvalidArgumentException(), 
                        ProductId = prod?.ID ?? throw new InvalidArgumentException(), 
                        TotalPrice = prod?.Price ?? throw new InvalidArgumentException()
                });
                cart.TotalPrice += prod?.Price ?? throw new InvalidArgumentException();
            }
            else
                throw new InvalidArgumentException("The requested product is out of stock.\n");
        }
        return cart;
    }
    /// <summary>
    /// updates a certain product's amount at the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="prodId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Cart UpdateProductAmount(BO.Cart cart, int prodId, int amount)
    {
        DO.Product? prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (Exception ex)
        {
            if (prodId > 999999 || prodId <= 99999)
                throw new InvalidArgumentException("id out of range.\n");
            throw new EntityNotFoundException("Requested Product to update amount not found.", ex);
        }
        OrderItem item = cart.Items!.Find(i => i.ProductId == prodId);
        if (item == null)
            throw new EntityNotFoundException("Item of requested product not found.\n");
        if (amount == 0 || amount * -1 > item.Amount )    //removes item
        { 
            cart.Items.Remove(item); 
            cart.TotalPrice -= item.Price;
        }
        else
        {
            //updates amount and price
            item.Amount += amount;
            item.TotalPrice += amount * prod?.Price ?? throw new InvalidArgumentException();
            cart.TotalPrice += amount * prod?.Price ?? throw new InvalidArgumentException();
        }
        return cart;
    }

    /// <summary>
    /// approves the order and makes the cart officially an order
    /// (includes updating all product's amount in stock)
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Order Approve(BO.Cart cart)
    {
        //order data validation
        if (!string.IsNullOrWhiteSpace(cart.CustomerName) && !string.IsNullOrWhiteSpace(cart.CustomerAddress) && IsValidEmail(cart.CustomerEmail!))
            foreach (BO.OrderItem item in cart.Items!)
            {
                try
                {
                    validItem(item);
                } 
                catch { throw; }
            }
        else
        {
            throw new InvalidArgumentException("One or more attributes of the cart order are invalid.\n");
        }
        BO.Order order = new BO.Order()//creates order
        {
            Items = cart.Items,
            CustomerAddress = cart.CustomerAddress,
            CustomerEmail = cart.CustomerEmail,
            CustomerName = cart.CustomerName,
            OrderDate = DateTime.Now,
            ShipDate = null,
            DeliveryDate = null,
            TotalPrice = cart.TotalPrice,
            Status = BO.orderStatus.Approved
        };
        int id = 0;
        try
        {
            id = dal.Order.Create(convertOrder(order));//founds sequence id and creates order in dl
        }
        catch (DO.DoubledEntityException ex){ throw new InvalidArgumentException("Unable to create Order.",ex); }
        order.Id = id;
        foreach (BO.OrderItem item in order.Items)//creates all items in dl and updates products amount in stock
        {
            dal.OrderItem.Create(convertOrderItem(item, id));
            item.ID = id;
            DO.Product product;
            try
            {
                product = dal.Product.RequestById(item.ProductId);
            }
            catch (Exception ex) { 
                if (item.ProductId > 999999 || item.ProductId <= 99999)
                    throw new InvalidArgumentException("id out of range.\n");
                throw new EntityNotFoundException("Requested Product for item creation not found.", ex);
            }

            if (item.Amount > 0 && product.InStock >= item.Amount)
                product.InStock -= item.Amount;
            else
                throw new InvalidArgumentException("Requested amount is greater than what's available.\n");
            dal.Product.Update(product);
        }
        return order;
    }

    /// <summary>
    /// help method, converts BO order to DO order
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private DO.Order convertOrder(BO.Order order)
    {
        return new DO.Order()
        {
            CustomerAddress = order.CustomerAddress,
            CustomerEmail = order.CustomerEmail,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            DeliveryDate = order.DeliveryDate,
            ShipDate = order.ShipDate
        };
    }
    /// <summary>
    /// help method, converts BO order item to DO order item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private DO.OrderItem convertOrderItem(BO.OrderItem item, int id)
    {
        return new DO.OrderItem()
        {
            OrderID = id,
            ProductID = item.ProductId,
            Price = item.Price,
            Amount = item.Amount
        };
    }

    /// <summary>
    /// empty items from cart
    /// </summary>
    /// <param name="cart"></param>
    public void Empty(BO.Cart cart)
    {
        cart!.Items!.Clear();
        cart.TotalPrice = 0;
    }

    /// <summary>
    /// checks if string is a valid email address
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private bool IsValidEmail(string s) => new EmailAddressAttribute().IsValid(s);

    /// <summary>
    /// help method, makes sure that the item exists and the requested amount is in stock
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    private bool validItem(BO.OrderItem item)
    {
        DO.Product? prod;
        try
        {
            prod = dal.Product.RequestById(item.ProductId);
        }
        catch (Exception ex)
        {
            if (item.ProductId > 999999 || item.ProductId <= 99999)
                throw new InvalidArgumentException("id out of range.\n");
            throw new EntityNotFoundException("Requested Product not found.", ex);
        }

        if (item.Amount > 0 && prod?.InStock >= item.Amount)
            return true;
        else
            throw new InvalidArgumentException("Requested amount is greater than what's available.\n");
    }
}
