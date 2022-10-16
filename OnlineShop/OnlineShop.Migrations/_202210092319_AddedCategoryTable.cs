using FluentMigrator;

namespace OnlineShop.Migrations
{
    [Migration(202210092319)]
    public class _202210092319_AddedCategoryTable : Migration
    {
        public override void Up()
        {
            Create.Table("ProductCategories")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("ParentId").AsInt32().NotNullable()
                .ForeignKey("FK_ProductCategories_ProductCategories",
                "ProductCategories",
                "Id");
        }

        public override void Down()
        {
            Delete.Table("ProductCategories");
        }
    }
}