using Rentaly.EntityLayer.Enums;

namespace Rentaly.DtoLayer.PageDtos;

public class HomeContentsIndexDto
{
    public List<HomeContentItemDto> AllItems { get; set; } = [];
    public List<HomeContentItemDto> Items { get; set; } = [];
    public HomeSectionType? SelectedSection { get; set; }

    public int Count(HomeSectionType section) => AllItems.Count(x => x.Section == section);
}
