using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Services.CategoryServices.Contracts;
using System.Linq;

namespace OnlineShop.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly DbSet<Category> _categories;
        public EFCategoryRepository(EFDataContext context)
        {
            _categories = context.Categories;
        }
        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public Task<bool> IsExistName(string name)
        {
            return _categories.AnyAsync(c => c.Name == name);
        }

        public async Task<Category> IsNameNotFound(string name)
        {
            var category = await _categories
                .FirstOrDefaultAsync(_ => _.Name == name);
            return category;
        }
    }
}
