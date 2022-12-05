using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Specs.Infrastructures;
using Xunit;

namespace OnlineShop.Specs.CategoryTests.Update
{
    [Scenario("ویرایش دسته بندی کالا")]
    public class UpdateCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private UpdateCategoryDto? _dto;
        private ProductCategories? _category;
        private ProductCategories? _parentCategory;
        private ProductCategories? _newParentCategory;
        public UpdateCategories(ConfigurationFixture configuration)
            : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _categoryRepository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_unitOfWork, _categoryRepository);
        }

        [Given("یک دسته بندی با عنوان برقی در" +
            " فهرست دسته بندی ها وجود دارد")]
        [And("یک دسته بندی با عنوان تلفن همراه " +
            "در فهرست دسته بندی ها وجود دارد")]
        public async Task Given()
        {
            _parentCategory = new ProductCategories
            {
                Name = "برقی"
            };
            _category = new ProductCategories
            {
                Name = "لوازم خانگی",
                Parent = _parentCategory
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _newParentCategory = new ProductCategories
            {
                Name = "تلفن همراه"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_newParentCategory));
        }

        [When("دسته بندی لوازم خانکی که در دسته بندی پدر با عنوان  " +
     "برقی بوده را  به لوازم آرایش ویرایش می کنیم")]
        public async Task When()
        {
            _dto = new UpdateCategoryDto
            {
                Name = "لوازم آرایشی",
                ParentId = _newParentCategory.Id
            };

            await _sut.Update(_category!.Id, _dto);

        }

        [Then("باید تنها یک دسته بندی با عنوان لوازم آرایشی " +
            "پدر دسته بندی پدر  تلفن همراه باید وجود داشته باشد")]
        public async Task Then()
        {
            var actual = await _context.ProductCategories.Where(_ => _.Name ==
            _dto!.Name && _.ParentId == _dto.ParentId).ToListAsync();
            actual.Should().HaveCount(1);
            actual.First().Name.Should().Be(_dto!.Name);
            actual.First().ParentId.Should().Be(_dto.ParentId);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given().Wait(),
                  _ => When().Wait(),
                 _ => Then().Wait());
        }
    }
}