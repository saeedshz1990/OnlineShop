using OnlineShop.Entities.Category;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.TestTools.Categories
{
    public static class CreateCategoryFactory
    {
        public static ProductCategories CreateCategoryDto(string name = "dummy")
        {
            return new ProductCategories
            {
                Name = name,
            };
        }

        public static AddCategoryDto CreateAddCategoryDto(
            int? parentId = null,
            string name = "dummy")
        {
            return new AddCategoryDto
            {
                Name = name,
                ParentId = parentId
            };
        }

        public static UpdateCategoryDto UpdateCategoryDto(
            int? parentId = null,
            string name = "UpdatedDummy")
        {
            return new UpdateCategoryDto
            {
                Name = name,
                ParentId = parentId
            };
        }
    }
}
