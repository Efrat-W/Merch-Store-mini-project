

namespace BO;

public class ProductForList
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public category? Category { get; set; }

    public override string ToString() => $@"
    Product id: {ID}
    Product Name: {Name}
    Category: {Category}
    Price: {Price}";
}
