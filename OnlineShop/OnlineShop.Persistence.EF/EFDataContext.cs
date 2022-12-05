using Microsoft.EntityFrameworkCore;
using OnlineShop.Entities.Category;
using OnlineShop.Persistence.EF.Categories;

namespace OnlineShop.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public DbSet<ProductCategories> ProductCategories { get; set; }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        public EFDataContext(string connectionString) :
           this(new DbContextOptionsBuilder()
               .UseSqlServer(connectionString).Options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }
    }
}
