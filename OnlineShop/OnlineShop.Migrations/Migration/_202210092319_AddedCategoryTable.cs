namespace OnlineShop.Migrations.Migration;

public class _202210092319_AddedCategoryTable :FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString(100).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Categories");
    }
}