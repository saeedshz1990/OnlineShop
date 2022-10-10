using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Services.CategoryServices.Exceptions;

namespace OnlineShop.Services.CategoryServices;

public class CategoryAppService : CategoryService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly CategoryRepository _repository;
    public CategoryAppService(UnitOfWork unitOfWork, CategoryRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Add(AddCategoryDto dto)
    {
        var name = await _repository.IsExistName(dto.Name);
        if (name)
        {
            throw new TheNameIsExistException();
        }
        var nameChecked = await _repository.IsNameNotFound(dto.Name);
        if (nameChecked == null)
        {
            throw new TheNameCanNotBeNullException();
        }

        var category = new Category()
        {
            Name = dto.Name,
        };

        _repository.Add(category);
        await _unitOfWork.Complete();
    }
}