using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnlineShop.Entities.Category;
using OnlineShop.Persistence.EF.Categories;

namespace OnlineShop.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }





        public override ChangeTracker ChangeTracker
        {
            get
            {
                var tracker = base.ChangeTracker;
                tracker.LazyLoadingEnabled = false;
                tracker.AutoDetectChangesEnabled = true;
                tracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                return tracker;
            }
        }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }
        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }
    }
}
