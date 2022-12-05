namespace OnlineShop.Services.CategoryServices.Contracts.Dto
{
    public class GetAllCategoriesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public HashSet<GetAllCategoriesDto> Child { get; set; }
    }
}
