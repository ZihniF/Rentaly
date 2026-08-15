using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260815120000_ConfigureLiveHomeStatistics")]
public class ConfigureLiveHomeStatistics : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE [HomeContents]
            SET [Icon] = CASE [DisplayOrder]
                WHEN 1 THEN N'completed-rentals'
                WHEN 2 THEN N'customers'
                WHEN 3 THEN N'active-cars'
                WHEN 4 THEN N'branches'
            END
            WHERE [Section] = N'Statistic'
              AND ([Icon] IS NULL OR [Icon] = N'')
              AND [DisplayOrder] BETWEEN 1 AND 4;

            UPDATE [HomeContents]
            SET [Title] = N'Şube sayısı'
            WHERE [Section] = N'Statistic'
              AND [DisplayOrder] = 4
              AND [Title] = N'Yıllık deneyim';
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE [HomeContents]
            SET [Icon] = NULL
            WHERE [Section] = N'Statistic'
              AND [Icon] IN (N'completed-rentals', N'customers', N'active-cars', N'branches');

            UPDATE [HomeContents]
            SET [Title] = N'Yıllık deneyim'
            WHERE [Section] = N'Statistic'
              AND [DisplayOrder] = 4
              AND [Title] = N'Şube sayısı';
            """);
    }
}
