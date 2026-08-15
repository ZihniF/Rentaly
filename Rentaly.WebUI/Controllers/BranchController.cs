using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers;

public class BranchController : Controller
{
    private readonly IBranchService _branchService;
    public BranchController(IBranchService branchService) => _branchService = branchService;

    public async Task<IActionResult> BranchList() => View("BranchList", await _branchService.TGetListAsync());
    public Task<IActionResult> Index() => BranchList();

    public async Task<IActionResult> Detail(int id)
    {
        try { return View(await _branchService.TGetByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet] public IActionResult CreateBranch() => View();
    [HttpGet] public IActionResult Create() => RedirectToAction(nameof(CreateBranch));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBranch(Branch branch)
    {
        if (!ModelState.IsValid) return View("CreateBranch", branch);

        try
        {
            branch.IsActive = true;
            await _branchService.TInsertAsync(branch);
            TempData["Success"] = "Şube eklendi.";
            return RedirectToAction(nameof(BranchList));
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Şube kaydedilemedi. Bilgileri kontrol edip tekrar deneyin.");
            return View("CreateBranch", branch);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(Branch branch) => CreateBranch(branch);

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try { return View(await _branchService.TGetByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Branch branch)
    {
        if (!ModelState.IsValid) return View(branch);

        try
        {
            var existing = await _branchService.TGetByIdAsync(branch.BranchId);
            existing.BranchName = branch.BranchName.Trim();
            existing.City = branch.City.Trim();
            existing.Address = branch.Address.Trim();
            await _branchService.TUpdateAsync(existing);
            TempData["Success"] = "Şube güncellendi.";
            return RedirectToAction(nameof(BranchList));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Şube güncellenemedi. Bilgileri kontrol edip tekrar deneyin.");
            return View(branch);
        }
    }

    [HttpPost("/Branch/Delete/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _branchService.TDeleteAsync(id);
            TempData["Success"] = "Şube silindi. Şubeye bağlı araçlar pasif duruma getirildi.";
        }
        catch (KeyNotFoundException)
        {
            TempData["Error"] = "Silinecek şube bulunamadı.";
        }
        catch
        {
            TempData["Error"] = "Şube silinemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction(nameof(BranchList));
    }
}
