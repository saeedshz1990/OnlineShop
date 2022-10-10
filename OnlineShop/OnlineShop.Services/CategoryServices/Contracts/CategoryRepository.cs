using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;

namespace OnlineShop.Services.CategoryServices.Contracts;

public interface CategoryRepository : Repository
{
    void Add(Category category);
    Task<bool> IsExistName(string name);
    Task<Category> IsNameNotFound(string name);


}