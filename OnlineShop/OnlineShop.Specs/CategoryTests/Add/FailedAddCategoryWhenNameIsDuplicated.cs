using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Specs.Infrastructures;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using OnlineShop.Services.CategoryServices.Exceptions;

namespace OnlineShop.Specs.CategoryTests.Add
{
    [Scenario("ثبت دسته بندی کالا")]
    public class FailedAddCategoryWhenNameIsDuplicated
        : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private AddCategoryDto _dto;
        private ProductCategories _category;
        Func<Task> _expected;

        public FailedAddCategoryWhenNameIsDuplicated(
            ConfigurationFixture configuration) :
            base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _repository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_unitOfWork, _repository);
        }

        [Given("دسته بندی با عنوان ‘برقی’ در لیست دسته بندی ها موجود می باشد")]
        private void Given()
        {
            _category = new ProductCategories()
            {
                Name = "برقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
        }

        [When("دسته بندی با عنوان ‘برقی’ تعریف می کنم")]
        private async Task When()
        {
            _dto = new AddCategoryDto
            {
                Name = "برقی"
            };

            _expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک دسته بندی با عنوان ‘برقی’ " +
            "در فهرست دسته بندی کالا باید وجود داشته باشد")]
        private async Task Then()
        {
            await _expected.Should().ThrowExactlyAsync<TheCategoryNameIsExistException>();

            var actual = await _context.ProductCategories.ToListAsync();
            actual.Should().Contain(_ => _.Name == _dto.Name);
        }

        [Fact]
        public void Run()
        {
            Given();
            When().Wait();
            Then().Wait();
        }
    }
}
