using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Services.CategoryServices.Contracts;

namespace OnlineShop.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly DbSet<Category> _categories;
        public EFCategoryRepository(EFDataContext context)
        {
            _categories = context.ProductCategories;
        }
        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public async Task<Category?> Find(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task<bool> IsExist(int id, int? parentId, string name)
        {
            var a= await _categories
                .AnyAsync(_ => _.Id == id &&
                _.ParentId == parentId &&_.Name == name);
            return a;
        }

        public Task<bool> IsExistName(string name)
        {
            return _categories.AnyAsync(c => c.Name == name);
        }

        public async Task<Category?> IsNameNotFound(string name)
        {
            return await _categories
                .FirstOrDefaultAsync(_ => _.Name == name);
        }

        public void Update(Category category)
        {
            
        }
    }
}
