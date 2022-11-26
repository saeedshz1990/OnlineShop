using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Specs.Infrastructures;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using Xunit;
using FluentAssertions;
using OnlineShop.Services.CategoryServices.Exceptions;

namespace OnlineShop.Specs.CategoryTests.Update
{
    public class FailedWhenCategoryNameIsExists : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private UpdateCategoryDto? _dto;
        private Category? _category;
        private Category? _newParentCategory;
        private Func<Task> actualResult;

        public FailedWhenCategoryNameIsExists(
            ConfigurationFixture configuration)
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
            _category = new Category
            {
                Name = "لوازم خانگی",
                Parent = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _newParentCategory = new Category
            {
                Name = "تلفن همراه",
                Parent = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_newParentCategory));
        }

        [When(" دسته بندی با عنوان  لوازم" +
            " خانگی را به تلفن همراه ویرایش می کنم")]
        public async Task When()
        {
            _dto = new UpdateCategoryDto
            {
                Name = "تلفن همراه",
                ParentId = null
            };

            actualResult = () => _sut.Update(_category!.Id, _dto);
        }

        [Then("بنابراین پیغام خطایی با عنوان  " +
            "نام دسته بندی تکراری می باشد به کاربر نمایش می دهد")]
        public async Task Then()
        {
            await actualResult.Should()
                .ThrowExactlyAsync<TheCategoryNameIsExistException>();
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
