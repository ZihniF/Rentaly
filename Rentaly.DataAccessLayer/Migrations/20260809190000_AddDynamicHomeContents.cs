using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentaly.DataAccessLayer.Concrete;

#nullable disable

namespace Rentaly.DataAccessLayer.Migrations;

[DbContext(typeof(RentalyContext))]
[Migration("20260809190000_AddDynamicHomeContents")]
public class AddDynamicHomeContents : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HomeContents",
            columns: table => new
            {
                HomeContentId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Section = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DisplayOrder = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_HomeContents", x => x.HomeContentId));

        migrationBuilder.Sql("""
            INSERT INTO [HomeContents] ([Section], [Title], [Subtitle], [Description], [ImageUrl], [Icon], [DisplayOrder], [IsActive]) VALUES
            (N'Process', N'Aracını seç', NULL, N'İhtiyacınıza uygun aracı filomuzdan seçin.', NULL, N'car', 1, 1),
            (N'Process', N'Lokasyon ve tarihi belirle', NULL, N'Teslim noktanızı ve yolculuk tarihlerinizi girin.', NULL, N'calendar', 2, 1),
            (N'Process', N'Rezervasyonunu tamamla', NULL, N'Talebinizi gönderin, onay e-postanızla yola çıkın.', NULL, N'check', 3, 1),
            (N'Future', N'Bugünün yolculuğunu yarının teknolojisiyle buluşturuyoruz.', NULL, N'Daha düşük emisyonlu araçlar, dijital rezervasyon ve müşteriyi merkeze alan hizmet anlayışıyla filomuzu her gün ileri taşıyoruz.', NULL, NULL, 1, 1),
            (N'Statistic', N'Tamamlanan rezervasyon', N'15.425+', NULL, NULL, NULL, 1, 1),
            (N'Statistic', N'Mutlu misafir', N'8.745+', NULL, NULL, NULL, 2, 1),
            (N'Statistic', N'Araçlık filo', N'235', NULL, NULL, NULL, 3, 1),
            (N'Statistic', N'Yıllık deneyim', N'15', NULL, NULL, NULL, 4, 1),
            (N'Award', N'Yılın araç kiralama markası', N'2026', N'Müşteri deneyimi ve hizmet kalitesinde sektör ödülü.', NULL, NULL, 1, 1),
            (N'Award', N'En iyi dijital deneyim', N'2026', N'Kolay ve güvenli rezervasyon akışıyla jüri özel ödülü.', NULL, NULL, 2, 1),
            (N'Award', N'Sürdürülebilir filo', N'2025', N'Düşük emisyonlu dönüşüm programı başarı ödülü.', NULL, NULL, 3, 1),
            (N'Testimonial', N'Selin Yılmaz', N'İstanbul', N'Araç tertemizdi, rezervasyon süreci birkaç dakika sürdü.', N'/rentaly/images/testimonial/1.jpg', NULL, 1, 1),
            (N'Testimonial', N'Mert Kaya', N'Ankara', N'Tarih değişikliğinde ekip çok hızlı yardımcı oldu. Kesinlikle tekrar tercih ederim.', N'/rentaly/images/testimonial/2.jpg', NULL, 2, 1),
            (N'Testimonial', N'Deniz Aksoy', N'İzmir', N'Havalimanı teslimi tam zamanında ve sorunsuz gerçekleşti.', N'/rentaly/images/testimonial/3.jpg', NULL, 3, 1),
            (N'Faq', N'Rezervasyon ne zaman kesinleşir?', NULL, N'Talebiniz admin tarafından onaylandığında rezervasyonunuz kesinleşir ve detaylı onay e-postası gönderilir.', NULL, NULL, 1, 1),
            (N'Faq', N'Araç seçtiğim tarihlerde nasıl korunur?', NULL, N'Bekleyen veya onaylı bir rezervasyonla çakışan tarih aralığında araç başka kullanıcıların sonuçlarında gösterilmez.', NULL, NULL, 2, 1),
            (N'Faq', N'Kimlik doğrulaması gerekiyor mu?', NULL, N'Hayır. Bu rezervasyon akışında çevrimiçi kimlik doğrulaması bulunmaz.', NULL, NULL, 3, 1),
            (N'Faq', N'Rezervasyonumu iptal edebilir miyim?', NULL, N'Müşteri hizmetlerimizle iletişime geçerek koşullar dahilinde iptal talebi oluşturabilirsiniz.', NULL, NULL, 4, 1);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("HomeContents");
}
