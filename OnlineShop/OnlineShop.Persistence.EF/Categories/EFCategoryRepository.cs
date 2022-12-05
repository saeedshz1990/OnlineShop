using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly DbSet<ProductCategories> _categories;
        public EFCategoryRepository(EFDataContext context)
        {
            _categories = context.ProductCategories;
        }
        public void Add(ProductCategories category)
        {
            _categories.Add(category);
        }

        public void Delete(ProductCategories category)
        {
            _categories.Remove(category);
        }

        public async Task<ProductCategories?> Find(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task<List<GetAllCategoriesDto>> GetAll()
        {
            var result= await _categories
                .Select(_ => new GetAllCategoriesDto
                {
                    Id = _.Id,
                    Title = _.Name,
                }).ToListAsync();
            return result;
        }

        public async Task<List<GetCategoryWithChildDto>> GetById(int categoryId)
        {
            var result= await _categories
                .Where(_ => _.Id == categoryId)
                .Select(_ => new GetCategoryWithChildDto
                {
                    Id = _.Id,
                    Title = _.Name
                }).ToListAsync();
            return result;
        }

        public async Task<bool> IsExist(int id, int? parentId, string name)
        {
            var a = await _categories
                .AnyAsync(_ => _.Id == id &&
                _.ParentId == parentId && _.Name == name);
            return a;
        }

        public Task<bool> IsExistName(string name)
        {
            return _categories.AnyAsync(c => c.Name == name);
        }

        public async Task<ProductCategories?> IsNameNotFound(string name)
        {
            return await _categories
                .FirstOrDefaultAsync(_ => _.Name == name);
        }

        public void Update(ProductCategories category)
        {

        }
    }
}
