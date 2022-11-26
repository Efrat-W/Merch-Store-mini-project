

namespace BO;

public enum category
{
    Prints, Apparel, Stickers, Notebooks, Mugs
}

public enum orderStatus
{
   Approved, Shipped, Delivered
}
public enum menu
{
    Exit, Product, Order, Cart
}

public enum optionsProduct
{
    Return, Add, ShowByIdMan, ShowByIdCus,Update, Delete
}
public enum optionsOrder
{
    Return, Track , ShowById, UpdateShipment, UpdateDelivery, RequestAll
}

public enum optionsCart
{
    Return, Add, UpdateAmount, Approve
}