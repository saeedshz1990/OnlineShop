using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;

namespace OnlineShop.RestApi.Controllers.Categories;

[ApiController]
[Route("api/categories")]
public class CategoryController: ControllerBase
{
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
        service = _service;
    }

    [HttpPost]
    public async Task Add(AddCategoryDto dto)
    {
        await _service.Add(dto);
    }
    
}