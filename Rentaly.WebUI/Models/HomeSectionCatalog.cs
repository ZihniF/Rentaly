using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.WebUI.Models;

public static class HomeSectionCatalog
{
    public static readonly IReadOnlyList<HomeSectionDefinition> All =
    [
        new(HomeSectionType.Process, "Process / İşleyiş", "Rezervasyon sürecinin adımlarını yönetin.", "fa-list-check"),
        new(HomeSectionType.Future, "Our Future", "Markanın gelecek vizyonunu yönetin.", "fa-seedling"),
        new(HomeSectionType.Statistic, "İstatistikler / About", "Ana sayfadaki sayısal göstergeleri yönetin.", "fa-chart-column"),
        new(HomeSectionType.Award, "Ödüller", "Kazanılan ödül ve başarıları yönetin.", "fa-award"),
        new(HomeSectionType.Testimonial, "Testimonial", "Müşteri görüşlerini ve görsellerini yönetin.", "fa-quote-left"),
        new(HomeSectionType.Faq, "Sık Sorulan Sorular", "Soru ve cevap içeriklerini yönetin.", "fa-circle-question")
    ];

    public static string DisplayName(this HomeSectionType section) =>
        typeof(HomeSectionType).GetMember(section.ToString()).First()
            .GetCustomAttribute<DisplayAttribute>()?.Name ?? section.ToString();
}

public sealed record HomeSectionDefinition(
    HomeSectionType Section,
    string Name,
    string Description,
    string Icon);
