using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastructure;
using OnlineShop.Persistence.EF;
using OnlineShop.Persistence.EF.Categories;
using OnlineShop.Services.CategoryServices;
using OnlineShop.Services.CategoryServices.Contracts;
using OnlineShop.Services.CategoryServices.Contracts.Dto;
using OnlineShop.Specs.Infrastructures;
using Xunit;

namespace OnlineShop.Specs.CategoryTests.Add;

[Scenario("ثبت دسته بندی کالا")]
public class AddCategories : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CategoryRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    private readonly CategoryService _sut;
    private AddCategoryDto _dto;

    public AddCategories(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _unitOfWork = new EFUnitOfWork(_context);
        _repository = new EFCategoryRepository(_context);
        _sut = new CategoryAppService(_unitOfWork, _repository);
    }

    [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
    private void Given()
    {

    }

    [When("دسته بندی با عنوان ‘برقی’ تعریف می کنم")]
    private async Task When()
    {
        _dto = new AddCategoryDto
        {
            Name = "برقی"
        };

        await _sut.Add(_dto);
    }
    [Then("دسته بندی با عنوان ‘برقی’ در فهرست دسته بندی کالا باید وجود داشته باشد")]
    private async Task Then()
    {
        var expected = await _context.ProductCategories
            .ToListAsync();
        expected!.First().Name.Should().Be(_dto.Name);
        expected.First().ParentId.Should().Be(null);
    }

    [Fact]
    public void Run()
    {
        Given();
        When().Wait();
        Then().Wait();
    }
}