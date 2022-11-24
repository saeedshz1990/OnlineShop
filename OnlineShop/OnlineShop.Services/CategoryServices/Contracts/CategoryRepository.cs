using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;

namespace OnlineShop.Services.CategoryServices.Contracts;

public interface CategoryRepository : Repository
{
    void Add(Category category);
    Task<Category?> Find(int id);
    Task<bool> IsExist(int id, int? parentId, string name);
    Task<bool> IsExistName(string name);
    Task<Category?> IsNameNotFound(string name);
    void Update(Category category);

}