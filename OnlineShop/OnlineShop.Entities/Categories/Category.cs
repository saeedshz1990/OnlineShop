namespace OnlineShop.Entities.Category;

public class Category
{
    public Category()
    {
        Child = new HashSet<Category>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public HashSet<Category> Child { get; set; }
}