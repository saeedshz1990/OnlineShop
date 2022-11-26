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

namespace OnlineShop.Specs.CategoryTests.Delete
{
    public class DeleteChildCategoryInParent 
        :EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private Category _category;
        private Category _childCategory;
        private Category _thirdCategory;
        private Category _secondCategory;

        public DeleteChildCategoryInParent(
            ConfigurationFixture configuration) 
            : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            _repository = new EFCategoryRepository(_context);
            _sut = new CategoryAppService(_unitOfWork, _repository);
        }
        [Given("یک دسته بندی با نام تجهیزات " +
           "برقی در فهرست دسته بندی موجود می باشد")]
        public async Task Given()
        {
            _category = new Category
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _childCategory = new Category
            {
                ParentId = _category.Id,
                Name = "جاروبرقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_childCategory));
            _thirdCategory = new Category
            {
                ParentId = _category.Id,
                Name = "یخچال و فریزر"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_thirdCategory));
            _secondCategory = new Category
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));
        }

        [When("در دسته بندی تجهیزات برقی " +
            " دسته بندی یخچال و فریزر را پاک می کنم")]
        public async Task When()
        {
            await _sut.Delete(_thirdCategory.Id);
        }

        [Then("بنابراین فقط یک دسته بندی با عنوان " +
            "آموزشی در فهرست دسته بندی وجود داشته باشد")]
        public async Task Then()
        {
            var actualResult = await _context.ProductCategories
               .SingleOrDefaultAsync();
            actualResult!.Name.Should().Be(_secondCategory.Name);
            actualResult.Child.Should().NotBeNull();
            actualResult.ParentId.Should().Be(_secondCategory.ParentId);
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
