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
    public CategoryAppService(
        UnitOfWork unitOfWork,
        CategoryRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<int> Add(AddCategoryDto dto)
    {
        var name = await _repository.IsExistName(dto.Name);
        StopIfCategoryNameIsExist(name);

        var category = new Category()
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        _repository.Add(category);
        await _unitOfWork.Complete();
        return category.Id;
    }

    public async Task Update(int id, UpdateCategoryDto dto)
    {
        var category = await _repository.Find(id);
        StopIfCategoryNotFound(category);

        await StopIfCategoryNameIsExist(id, dto, category);

        category!.Name = dto.Name;
        category.ParentId = dto.ParentId;

        _repository.Update(category);

        await _unitOfWork.Complete();
    }

    private async Task StopIfCategoryNameIsExist(
        int id,
        UpdateCategoryDto dto,
        Category? category)
    {
        if (await _repository.IsExist(id, category.ParentId, dto.Name))
        {
            throw new TheCategoryNameIsExistException();
        }
    }

    private static void StopIfCategoryNotFound(Category? category)
    {
        if (category == null)
        {
            throw new ThisCategoryNotFoundException();
        }
    }

    private static void StopIfCategoryNameIsExist(bool name)
    {
        if (name)
        {
            throw new TheCategoryNameIsExistException();
        }
    }

    public async Task Delete(int id)
    {
        var category = await _repository.Find(id);
        if (category == null)
        {
            throw new ThisCategoryNotFoundException();
        }

        _repository.Delete(category!);
       await _unitOfWork.Complete();
    }
}