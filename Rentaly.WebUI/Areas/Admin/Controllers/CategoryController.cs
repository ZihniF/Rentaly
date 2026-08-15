using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

    public async Task<IActionResult> CategoryList() => View("CategoryList", await _categoryService.TGetListAsync());
    [HttpGet("/Category/Index")] public Task<IActionResult> Index() => CategoryList();

    [HttpGet] public IActionResult CreateCategory() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        await _categoryService.TInsertAsync(category);
        return RedirectToAction(nameof(CategoryList));
    }

    [HttpPost("/Category/Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(CategoryList));
        await _categoryService.TInsertAsync(category);
        TempData["Success"] = "Kategori eklendi.";
        return RedirectToAction(nameof(CategoryList));
    }

    [HttpPost("/Category/Delete/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _categoryService.TDeleteAsync(id); TempData["Success"] = "Kategori silindi."; }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(CategoryList));
    }

    [HttpPost("/Category/Edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(CategoryList));
        await _categoryService.TUpdateAsync(category);
        TempData["Success"] = "Kategori güncellendi.";
        return RedirectToAction(nameof(CategoryList));
    }
}
