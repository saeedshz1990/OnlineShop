using OnlineShop.Entities.Category;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.TestTools.Categories
{
    public static class CreateCategoryFactory
    {
        public static Category CreateCategoryDto(string name="dummy")
        {
            return new Category
            {
                Name = name,
            };
        }

        public static AddCategoryDto CreateAddCategoryDto(string name="dummy")
        {
            return new AddCategoryDto
            {
                Name = name,
            };
        }
    }
}
