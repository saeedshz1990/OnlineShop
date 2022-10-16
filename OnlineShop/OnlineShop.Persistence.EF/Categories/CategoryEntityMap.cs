using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Entities.Category;

namespace OnlineShop.Persistence.EF.Categories
{
    public class CategoryEntityMap :
        IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> _)
        {
            _.ToTable("ProductCategories");
            _.HasKey(_ => _.Id);

            _.Property(_ => _.Id)
                .ValueGeneratedOnAdd();
            _.Property(_ => _.Name)
                .HasMaxLength(100)
                .IsRequired();

            _.HasOne(_ => _.Parent).WithMany(_ => _.Child)
            .HasForeignKey(_ => _.ParentId).IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
