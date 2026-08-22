using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260822120000_AddCategoryArchiving")]
public class AddCategoryArchiving : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Categories",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "IsActive", table: "Categories");
    }
}
