namespace OnlineShop.Entities.Category;

public class ProductCategories
{
    public ProductCategories()
    {
        Child = new HashSet<ProductCategories>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
    public ProductCategories? Parent { get; set; }
    public HashSet<ProductCategories> Child { get; set; }
}