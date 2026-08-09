using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.WebUI.Models;

public class HomeContentsIndexViewModel
{
    public List<HomeContent> AllItems { get; set; } = [];
    public List<HomeContent> Items { get; set; } = [];
    public HomeSectionType? SelectedSection { get; set; }

    public int Count(HomeSectionType section) => AllItems.Count(x => x.Section == section);
}
