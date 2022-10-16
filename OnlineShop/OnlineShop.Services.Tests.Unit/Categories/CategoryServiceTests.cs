using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.TestTools.Categories;
using Xunit;

namespace OnlineShop.Services.Tests.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _context;
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private AddCategoryDto _dto;
        private Category _category;

        public CategoryServiceTests()
        {
            _context = new EfInMemoryDatabase().CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_context);
            _categoryRepository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_unitOfWork, _categoryRepository);
        }

        [Fact]
        public async Task Add_add_Categories_properly()
        {
            _dto = CreateCategoryFactory
                .CreateAddCategoryDto(null, "Dummy");

            await _sut.Add(_dto);

            var actual = await _context.ProductCategories.ToListAsync();
            actual.Should().HaveCount(1);
            actual.First().Name.Should().Be(_dto.Name);
            actual.First().ParentId.Should().Be(null);
        }
    }
}
