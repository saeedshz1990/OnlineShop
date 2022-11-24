namespace OnlineShop.Services.CategoryServices.Contracts.Dto
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = default!;
        public int? ParentId { get; set; }
    }
}