using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents;

public class PublicFooterViewComponent : ViewComponent
{
    private readonly IHomePageSettingsService _settingsService;

    public PublicFooterViewComponent(IHomePageSettingsService settingsService) =>
        _settingsService = settingsService;

    public async Task<IViewComponentResult> InvokeAsync() => View(await _settingsService.TGetAsync());
}
