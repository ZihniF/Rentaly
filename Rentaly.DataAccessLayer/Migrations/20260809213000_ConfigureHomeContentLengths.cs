using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260809213000_ConfigureHomeContentLengths")]
public class ConfigureHomeContentLengths : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Title",
            table: "HomeContents",
            type: "nvarchar(180)",
            maxLength: 180,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Subtitle",
            table: "HomeContents",
            type: "nvarchar(180)",
            maxLength: 180,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "HomeContents",
            type: "nvarchar(1500)",
            maxLength: 1500,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ImageUrl",
            table: "HomeContents",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Icon",
            table: "HomeContents",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(name: "Title", table: "HomeContents", type: "nvarchar(max)", nullable: false,
            oldClrType: typeof(string), oldType: "nvarchar(180)", oldMaxLength: 180);
        migrationBuilder.AlterColumn<string>(name: "Subtitle", table: "HomeContents", type: "nvarchar(max)", nullable: true,
            oldClrType: typeof(string), oldType: "nvarchar(180)", oldMaxLength: 180, oldNullable: true);
        migrationBuilder.AlterColumn<string>(name: "Description", table: "HomeContents", type: "nvarchar(max)", nullable: true,
            oldClrType: typeof(string), oldType: "nvarchar(1500)", oldMaxLength: 1500, oldNullable: true);
        migrationBuilder.AlterColumn<string>(name: "ImageUrl", table: "HomeContents", type: "nvarchar(max)", nullable: true,
            oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500, oldNullable: true);
        migrationBuilder.AlterColumn<string>(name: "Icon", table: "HomeContents", type: "nvarchar(max)", nullable: true,
            oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100, oldNullable: true);
    }
}
