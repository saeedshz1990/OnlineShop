using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.Services.CategoryServices.Contracts;

public interface CategoryRepository : Repository
{
    void Add(ProductCategories category);
    Task<ProductCategories?> Find(int id);
    Task<bool> IsExist(int id, int? parentId, string name);
    Task<bool> IsExistName(string name);
    Task<ProductCategories?> IsNameNotFound(string name);
    void Update(ProductCategories category);
    void Delete(ProductCategories category);
    Task<List<GetAllCategoriesDto>> GetAll();
    Task<List<GetCategoryWithChildDto>> GetById(int categoryId);

}