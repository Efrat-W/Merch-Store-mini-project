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

    public BO.Cart AddProduct(BO.Cart cart, int prodId)
    {
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (Exception)
        {

            throw;
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
        else
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

    public BO.Cart UpdateProductAmount(BO.Cart cart, int prodId, int amount)
    {
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(prodId);
        }
        catch (Exception)
        {

            throw;
        }
        BO.OrderItem item = cart.Items.Find(i => i.ProductId == prodId);
        if (amount == 0)
            cart.Items.Remove(item);
        else
        {
            item.Amount += amount;
            item.TotalPrice += amount * prod.Price;
            cart.TotalPrice += amount * prod.Price;
        }

        return cart;
    }

    public BO.Order Approve(BO.Cart cart)
    {
        if (!string.IsNullOrEmpty(cart.CustomerName) && !string.IsNullOrEmpty(cart.CustomerAddress) && cart.CustomerEmail.Contains('@') && !cart.CustomerEmail.StartsWith('@'))
            try
            {
                foreach (BO.OrderItem item in cart.Items)
                {
                    if (!validItem(item))
                        throw new Exception();
                }
            }
            catch (Exception)
            {

                throw;
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
        catch { }
        foreach (BO.OrderItem item in order.Items)
        {
            dal.OrderItem.Create(convertOrderItem(item, id));
            item.ID = id;
            DO.Product product = dal.Product.RequestById(item.ProductId);
            product.InStock -= item.Amount;
            dal.Product.Update(product);
        }
        return order;
    }

    //auxillary methods
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
    private bool validItem(BO.OrderItem item)
    {
        DO.Product prod;
        try
        {
            prod = dal.Product.RequestById(item.ProductId);
        }
        catch (Exception)
        {

            throw;
        }

        if (item.Amount > 0 && prod.InStock >= item.Amount)
            return true;
        return false;
    }
}
