using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260822123000_AddHomePageSettings")]
public class AddHomePageSettings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE [HomePageSettings] (
                [HomePageSettingsId] int NOT NULL CONSTRAINT [PK_HomePageSettings] PRIMARY KEY,
                [HeroEyebrow] nvarchar(160) NOT NULL, [HeroTitle] nvarchar(200) NOT NULL,
                [HeroAccentText] nvarchar(120) NOT NULL, [HeroSecondLine] nvarchar(200) NOT NULL,
                [HeroDescription] nvarchar(500) NOT NULL, [HeroBackgroundUrl] nvarchar(500) NOT NULL,
                [ProcessEyebrow] nvarchar(120) NOT NULL, [ProcessTitle] nvarchar(180) NOT NULL,
                [CarsEyebrow] nvarchar(120) NOT NULL, [CarsTitle] nvarchar(180) NOT NULL,
                [FutureEyebrow] nvarchar(120) NOT NULL, [FutureBackgroundUrl] nvarchar(500) NOT NULL,
                [StatisticsEyebrow] nvarchar(120) NOT NULL, [StatisticsTitle] nvarchar(180) NOT NULL,
                [BrandsEyebrow] nvarchar(120) NOT NULL, [BrandsTitle] nvarchar(180) NOT NULL,
                [AwardsEyebrow] nvarchar(120) NOT NULL, [AwardsTitle] nvarchar(180) NOT NULL,
                [TestimonialsEyebrow] nvarchar(120) NOT NULL, [TestimonialsTitle] nvarchar(180) NOT NULL,
                [FaqEyebrow] nvarchar(120) NOT NULL, [FaqTitle] nvarchar(180) NOT NULL,
                [FaqDescription] nvarchar(400) NOT NULL, [FooterDescription] nvarchar(500) NOT NULL,
                [ContactPhone] nvarchar(80) NOT NULL, [ContactEmail] nvarchar(160) NOT NULL,
                [WorkingHours] nvarchar(160) NOT NULL, [RoadsideAssistance] nvarchar(160) NOT NULL
            );

            INSERT INTO [HomePageSettings] VALUES (
                1, N'Yolculuğun özgür hali', N'Aradığınız', N'araç', N'tam burada.',
                N'Lokasyonunuzu ve tarihlerinizi seçin, yalnızca gerçekten müsait araçları görün.',
                N'/rentaly/images/background/1.jpg', N'Process / İşleyiş', N'Üç adımda yola çıkın',
                N'Filomuz', N'Araçlar', N'Our Future / Geleceğimiz', N'/rentaly/images/background/3.jpg',
                N'About / İstatistikler', N'Rakamlarla Rentaly', N'Markalar & Modeller', N'Filomuzu keşfedin',
                N'Başarılarımız', N'Ödüller', N'Testimonial', N'Misafirlerimizin deneyimleri',
                N'Yardım', N'Sık Sorulan Sorular', N'Rezervasyon öncesi merak ettikleriniz.',
                N'Kaliteli araçlar, kolay rezervasyon ve güvenli yolculuk.', N'+90 850 000 00 00',
                N'hello@rentaly.com', N'Hafta içi 08.00–18.00', N'7/24 yol yardım'
            );
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable(name: "HomePageSettings");
}
