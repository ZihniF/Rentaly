using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260823100000_AddCustomerArchiving")]
public class AddCustomerArchiving : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Customers",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "IsActive", table: "Customers");
    }
}
