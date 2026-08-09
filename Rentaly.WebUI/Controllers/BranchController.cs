using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers;

public class BranchController : Controller
{
    private readonly IBranchService _branchService;
    public BranchController(IBranchService branchService) => _branchService = branchService;

    public async Task<IActionResult> BranchList() => View(await _branchService.TGetListAsync());
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
        if (!ModelState.IsValid) return View(branch);
        await _branchService.TInsertAsync(branch);
        return RedirectToAction(nameof(BranchList));
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
        await _branchService.TUpdateAsync(branch);
        return RedirectToAction(nameof(BranchList));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _branchService.TDeleteAsync(id); TempData["Success"] = "Şube silindi."; }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(BranchList));
    }
}
