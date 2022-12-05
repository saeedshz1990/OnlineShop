using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Specs.Infrastructures;
using Xunit;

namespace OnlineShop.Specs.CategoryTests.GetAll
{
    [Scenario("نمایش دسته بندی کالا")]
    public class GetAllCategoriesWithChild : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private ProductCategories _category;
        private ProductCategories _newParentCategory;
        private ProductCategories _parentCategory;

        public GetAllCategoriesWithChild(
            ConfigurationFixture configuration) 
            : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _repository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_unitOfWork, _repository);
        }

        [Given("دسته بندی با عنوان آموزشی در دسته بندی کالا وجود دارد")]
        [And("یک زیر دسته بندی با عنوان " +
            "برنامه نویسی در دسته بندی آموزشی قرار دارد")]
        private void Given()
        {
            _parentCategory = new ProductCategories
            {
                Name = "آموزشی"
            };
            _category = new ProductCategories
            {
                Name = "برنامه نویسی",
                ParentId = _parentCategory.ParentId
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _newParentCategory = new ProductCategories
            {
                Name = "لوازم برقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_newParentCategory));
        }

        [When("درخواست نمایش همه دسته بندی کالا را میدهم")]
        private async Task When()
        {
            await _sut.GetAll();
        }

        [Then("بنابراین تمام دسته بندی ها رو به نمایش می دهد")]
        private async Task Then()
        {
            var actualResult =await _context.ProductCategories.ToListAsync();
            actualResult.Should().HaveCount(2);
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
