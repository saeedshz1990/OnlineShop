using OnlineShop.Persistence.EF;
using Xunit;

namespace OnlineShop.Specs.Infrastructures;

[Collection(nameof(ConfigurationFixture))]
public class EFDataContextDatabaseFixture : DatabaseFixture
{
    private readonly ConfigurationFixture _configuration;

    public EFDataContextDatabaseFixture(ConfigurationFixture configuration)
    {
        _configuration = configuration;
    }

    public EFDataContext CreateDataContext()
    {
        return new EFDataContext(_configuration.Value.DbConnectionString);
    }
}