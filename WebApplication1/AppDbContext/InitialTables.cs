using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;

[Migration(20250331181815)] 
public class InitialDatabaseSchema : Migration
{
    public override void Up()
    {
        Create.Table("user")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("description").AsString(500).Nullable()
            .WithColumn("phone").AsString(20).Nullable();

        Create.Table("book")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("style").AsString(50).Nullable()
            .WithColumn("author_id").AsInt32().ForeignKey("user", "id");

        Create.Table("favourites")
            .WithColumn("book_id").AsInt32().ForeignKey("book", "id")
            .WithColumn("user_id").AsInt32().ForeignKey("user", "id");

        Create.PrimaryKey("favourites_pk").OnTable("favourites")
            .Columns("book_id", "user_id");

        Create.Table("read_history")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().ForeignKey("user", "id")
            .WithColumn("book_id").AsInt32().ForeignKey("book", "id")
            .WithColumn("page").AsInt32().NotNullable()
            .WithColumn("update_date").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Delete.Table("read_history");
        Delete.Table("favourites");
        Delete.Table("book");
        Delete.Table("user");
    }
}