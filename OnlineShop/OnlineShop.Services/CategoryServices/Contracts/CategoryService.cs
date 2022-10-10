using OnlineShop.Infrastructure;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.Services.CategoryServices.Contracts;

public interface CategoryService : Service
{
    Task Add(AddCategoryDto dto);
}