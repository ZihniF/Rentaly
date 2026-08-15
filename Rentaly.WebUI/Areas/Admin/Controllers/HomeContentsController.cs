using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.PageDtos;
using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;
using Rentaly.WebUI.Mappings;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeContentsController : Controller
{
    private readonly IHomeContentService _service;

    public HomeContentsController(IHomeContentService service) => _service = service;

    public async Task<IActionResult> Index(HomeSectionType? section)
    {
        var allItems = (await _service.TGetListAsync())
            .OrderBy(x => x.Section)
            .ThenBy(x => x.DisplayOrder)
            .Select(x => x.ToPageDto())
            .ToList();

        return View(new HomeContentsIndexDto
        {
            AllItems = allItems,
            Items = section.HasValue
                ? allItems.Where(x => x.Section == section).ToList()
                : allItems,
            SelectedSection = section
        });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            return View(await _service.TGetByIdAsync(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public IActionResult Create(HomeSectionType? section) => View("Form", new HomeContent
    {
        Section = section ?? HomeSectionType.Process,
        DisplayOrder = 1,
        IsActive = true
    });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HomeContent model)
    {
        if (!ModelState.IsValid) return View("Form", model);

        await _service.TInsertAsync(model);
        TempData["Success"] = $"{model.Section.DisplayName()} içeriği eklendi.";
        return RedirectToAction(nameof(Index), new { section = model.Section });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            return View("Form", await _service.TGetByIdAsync(id));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(HomeContent model)
    {
        if (!ModelState.IsValid) return View("Form", model);

        await _service.TUpdateAsync(model);
        TempData["Success"] = $"{model.Section.DisplayName()} içeriği güncellendi.";
        return RedirectToAction(nameof(Index), new { section = model.Section });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var item = await _service.TGetByIdAsync(id);
            await _service.TDeleteAsync(id);
            TempData["Success"] = "Ana sayfa içeriği silindi.";
            return RedirectToAction(nameof(Index), new { section = item.Section });
        }
        catch (KeyNotFoundException)
        {
            TempData["Error"] = "Silmek istediğiniz içerik bulunamadı.";
            return RedirectToAction(nameof(Index));
        }
    }
}
