
namespace BO;

public class Product
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public category? Category { get; set; }
    public int InStock { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public override string ToString() => $@"
    Product id: {ID}
    Product Name: {Name}
    Category: {Category}
    Price: {Price}
    In Stock: {InStock}";

}
