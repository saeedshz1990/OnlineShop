using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Services.CategoryServices.Exceptions;
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
        private ProductCategories _category;
        private ProductCategories _parentCategory;
        private ProductCategories _secondCategory;
        private ProductCategories _childCategory;
        private ProductCategories _thirdCategory;
        private UpdateCategoryDto _update;

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

        [Fact]
        public async Task Add_add_exception_When_Name_is_duplicated_properly()
        {
            _category = CreateCategoryFactory.CreateCategoryDto("Dummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _dto = CreateCategoryFactory
                .CreateAddCategoryDto(null, "Dummy");

            var expected = () => _sut.Add(_dto);

            await expected.Should().ThrowExactlyAsync<
                TheCategoryNameIsExistException>();
        }

        [Fact]
        public async Task Update_update_category_properly()
        {
            _category = CreateCategoryFactory.CreateCategoryDto("Dummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _update = CreateCategoryFactory
                .UpdateCategoryDto(null, "UpdatedDummy");

            await _sut.Update(_category.Id, _update);

            var actual = await _context.ProductCategories.ToListAsync();
            actual.Should().HaveCount(1);
            actual.First().Name.Should().Be(_update.Name);
            actual.First().ParentId.Should().BeNull();
        }

        [Fact]
        public async Task Update_update_category_to_parent_properly()
        {
            _category = CreateCategoryFactory.CreateCategoryDto("Dummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _parentCategory = CreateCategoryFactory.CreateCategoryDto("NewDummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_parentCategory));
            _update = new UpdateCategoryDto
            {
                Name = "UpdatedDummy",
                ParentId = null
            };

            await _sut.Update(_category.Id, _update);

            var actual = await _context.ProductCategories.ToListAsync();
            actual.First().Name.Should().Be(_update.Name);
            actual.First().ParentId.Should().Be(_update.ParentId);
        }

        [Fact]
        public async Task Update_throw_exception_when_name_is_exist_properly()
        {
            _category = CreateCategoryFactory.CreateCategoryDto("Dummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _update = new UpdateCategoryDto
            {
                Name = "Dummy",
                ParentId = null
            };

            var actualResult = () => _sut.Update(_category.Id, _update);

            await actualResult.Should().ThrowExactlyAsync<
                 TheCategoryNameIsExistException>();
        }

        [Theory]
        [InlineData(-1)]
        public async Task Update_throw_exception_when_category_id_not_found_properly(
            int invalidId)
        {
            _category = CreateCategoryFactory.CreateCategoryDto("Dummy");
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _update = new UpdateCategoryDto
            {
                Name = "Dummy",
                ParentId = null
            };

            var actualResult = () => _sut.Update(invalidId, _update);

            await actualResult.Should().ThrowExactlyAsync<
                 ThisCategoryNotFoundException>();
        }

        [Fact]
        public async Task Delete_delete_Parent_category_properly()
        {
            _category = new ProductCategories
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _secondCategory = new ProductCategories
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));

            await _sut.Delete(_category.Id);

            var actualResult = await _context.ProductCategories
                .ToListAsync();
            actualResult.First().Name.Should().Be(_secondCategory.Name);
            actualResult.First().ParentId.Should().Be(_secondCategory.ParentId);
        }

        [Fact]
        public async Task Delete_delete_Category_when_is_Child_properly()
        {
            _category = new ProductCategories
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _childCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "جاروبرقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_childCategory));
            _secondCategory = new ProductCategories
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));

            await _sut.Delete(_category.Id);

            var actualResult = await _context.ProductCategories
               .SingleOrDefaultAsync();
            actualResult.Name.Should().Be(_secondCategory.Name);
            actualResult.ParentId.Should().Be(_secondCategory.ParentId);
        }

        [Fact]
        public async Task Delete_delete_Child_category_In_parent_properly()
        {
            _category = new ProductCategories
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _childCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "جاروبرقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_childCategory));
            _thirdCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "یخچال و فریزر"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_thirdCategory));
            _secondCategory = new ProductCategories
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));

            await _sut.Delete(_childCategory.Id);

            var actualResult = await _context.ProductCategories
             .SingleOrDefaultAsync();
            actualResult!.Name.Should().Be(_thirdCategory.Name);
            actualResult.ParentId.Should().Be(_thirdCategory.ParentId);
        }

        [Theory]
        [InlineData(-1)]
        public async Task
            Delete_throw_exception_when_category_id_not_found_properly(
            int invalidId)
        {
            var actualResult = () => _sut.Delete(invalidId);

            await actualResult.Should().ThrowExactlyAsync<
                 ThisCategoryNotFoundException>();
        }

        [Fact]
        public async Task Get_get_all_categories_properly()
        {
            _category = new ProductCategories
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _childCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "جاروبرقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_childCategory));
            _thirdCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "یخچال و فریزر"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_thirdCategory));
            _secondCategory = new ProductCategories
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));

            await _sut.GetAll();

            var actualResult = await _context.ProductCategories.ToListAsync();
            actualResult.Should().HaveCount(4);

        }

        [Fact]
        public async Task Get_get_by_id_categories_properly()
        {
            _category = new ProductCategories
            {
                Name = "تجهیزات برقی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_category));
            _childCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "جاروبرقی"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_childCategory));
            _thirdCategory = new ProductCategories
            {
                ParentId = _category.Id,
                Name = "یخچال و فریزر"
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_thirdCategory));
            _secondCategory = new ProductCategories
            {
                Name = "آموزشی",
                ParentId = null
            };
            _context.Manipulate(_ => _.ProductCategories.Add(_secondCategory));

            await _sut.GetById(_category.Id);

            var actualResult = _context.ProductCategories.Where(_ => _.Id == _category.Id).ToList();
            actualResult.Should().HaveCount(1);
        }
    }
}
