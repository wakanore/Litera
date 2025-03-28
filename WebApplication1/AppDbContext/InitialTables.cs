using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;

[Migration(20240328181815)] // Используем дату и время как версию миграции
public class InitialDatabaseSchema : Migration
{
    public override void Up()
    {
        // Таблица User
        Create.Table("User")
            .WithColumn("ID").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("description").AsString(500).Nullable()
            .WithColumn("phone").AsString(20).Nullable();

        // Таблица Book
        Create.Table("Book")
            .WithColumn("ID").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("style").AsString(50).Nullable()
            .WithColumn("ID_author").AsInt32().ForeignKey("User", "ID");

        // Таблица Favourites (многие-ко-многим)
        Create.Table("Favourites")
            .WithColumn("IDBook").AsInt32().ForeignKey("Book", "ID")
            .WithColumn("IDUser").AsInt32().ForeignKey("User", "ID");

        // Создаем составной первичный ключ для Favourites
        Create.PrimaryKey("PK_Favourites").OnTable("Favourites")
            .Columns("IDBook", "IDUser");

        // Таблица ReadHistory
        Create.Table("ReadHistory")
            .WithColumn("ID").AsInt32().PrimaryKey().Identity()
            .WithColumn("IDUser").AsInt32().ForeignKey("User", "ID")
            .WithColumn("IDBook").AsInt32().ForeignKey("Book", "ID")
            .WithColumn("Page").AsInt32().NotNullable()
            .WithColumn("UpdateDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        // Удаление таблиц в обратном порядке (из-за foreign key constraints)
        Delete.Table("ReadHistory");
        Delete.Table("Favourites");
        Delete.Table("Book");
        Delete.Table("User");
    }
}