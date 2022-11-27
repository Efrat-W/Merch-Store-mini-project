using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using Dal;
using DalApi;

namespace BlImplementation;

internal class Cart : ICart
{
    IDal dal = new DalList();
    /// <summary>
    /// adds a product to the customer's cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="prodId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    public BO.Cart AddProduct(BO.Cart cart, int prodId)
    {
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (Exception ex)
        {
            throw new InvalidArgumentException(ex);
        }
        if (cart.Items.FirstOrDefault(i => i.ProductId == prodId) != null) //product exists in items
        {
            BO.OrderItem item = cart.Items.Find(i => i.ProductId == prodId);
            if (prod.InStock > item.Amount)
            {
                item.Amount++;
                item.TotalPrice += prod.Price;
                cart.TotalPrice += prod.Price;
            }
            else
                throw new InvalidArgumentException("The amount requested is greater than available.\n");
        }
        else//product does not exist in cart
        {
            if (prod.InStock > 1)
            {
                cart.Items.Add(new BO.OrderItem { ID = 0, Amount = 1, Name = prod.Name, Price = prod.Price, ProductId = prod.ID, TotalPrice = prod.Price });
                cart.TotalPrice += prod.Price;
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
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (MissingEntityException ex)
        {
            throw new InvalidArgumentException(ex);
        }
        BO.OrderItem item;
        try
        {
            item = cart.Items.Find(i => i.ProductId == prodId);
        }
        catch { throw new EntityNotFoundException("the item is not in your cart"); }
        if (amount == 0 || amount >= -1*item.Amount)
            cart.Items.Remove(item);
        else
        {
            item.Amount += amount;
            item.TotalPrice += amount * prod.Price;
            cart.TotalPrice += amount * prod.Price;
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
        if (!string.IsNullOrEmpty(cart.CustomerName) && !string.IsNullOrEmpty(cart.CustomerAddress) && cart.CustomerEmail.Contains('@') && !cart.CustomerEmail.StartsWith('@'))
            foreach (BO.OrderItem item in cart.Items)
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
        BO.Order order = new BO.Order()
        {
            Items = cart.Items,
            CustomerAddress = cart.CustomerAddress,
            CustomerEmail = cart.CustomerEmail,
            CustomerName = cart.CustomerName,
            OrderDate = DateTime.Now,
            ShipDate = DateTime.MinValue,
            DeliveryDate = DateTime.MinValue,
            TotalPrice = cart.TotalPrice,
            Status = BO.orderStatus.Approved
        };
        int id = 0;
        try
        {
            id = dal.Order.Create(convertOrder(order));
        }
        catch (DoubledEntityException ex){ throw new InvalidArgumentException(ex); }
        foreach (BO.OrderItem item in order.Items)
        {
            dal.OrderItem.Create(convertOrderItem(item, id));
            item.ID = id;
            DO.Product product;
            try
            {
                product = dal.Product.RequestById(item.ProductId);
            }
            catch (Exception ex) { throw new InvalidArgumentException(ex); }

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
    /// help method, makes sure that the item exists and the requested amount is in stock
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="InvalidArgumentException"></exception>
    private bool validItem(BO.OrderItem item)
    {
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(item.ProductId);
        }
        catch (Exception ex)
        {
            throw new InvalidArgumentException(ex);
        }

        if (item.Amount > 0 && prod.InStock >= item.Amount)
            return true;
        else
            throw new InvalidArgumentException("Requested amount is greater than what's available.\n");
    }
}
