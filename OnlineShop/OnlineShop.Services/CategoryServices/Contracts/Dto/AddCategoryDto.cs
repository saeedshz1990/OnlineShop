namespace OnlineShop.Services.CategoryServices.Contracts.Dto;

public class AddCategoryDto
{
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
}