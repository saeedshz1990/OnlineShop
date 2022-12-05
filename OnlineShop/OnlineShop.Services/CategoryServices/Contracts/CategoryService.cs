using OnlineShop.Infrastructure;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.Services.CategoryServices.Contracts;

public interface CategoryService : Service
{
    Task<int> Add(AddCategoryDto dto);
    Task Update(int id,UpdateCategoryDto dto);
    Task Delete(int id);
    Task<List<GetAllCategoriesDto>> GetAll();
    Task<List<GetCategoryWithChildDto>> GetById(int categoryId);
}