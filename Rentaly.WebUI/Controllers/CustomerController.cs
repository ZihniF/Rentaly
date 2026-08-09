using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DtoLayer.CustomerDtos;

namespace Rentaly.WebUI.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService) => _customerService = customerService;

    public async Task<IActionResult> CustomerList() => View(await _customerService.TGetListAsync());

    public async Task<IActionResult> Detail(int id)
    {
        try { return View(await _customerService.TGetByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet]
    public IActionResult Create() => View("Form", new CreateCustomerDto { DrivingLicenseDate = DateTime.Today });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        if (!ModelState.IsValid) return View("Form", dto);
        await _customerService.TInsertAsync(dto);
        return RedirectToAction(nameof(CustomerList));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var x = await _customerService.TGetByIdAsync(id);
            return View("Edit", new UpdateCustomerDto { CustomerId = x.CustomerId, Name = x.Name,
                Surname = x.Surname, Email = x.Email, Phone = x.Phone, IdentityNumber = x.IdentityNumber,
                DrivingLicenseNumber = x.DrivingLicenseNumber, DrivingLicenseDate = x.DrivingLicenseDate });
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid) return View("Edit", dto);
        await _customerService.TUpdateAsync(dto);
        return RedirectToAction(nameof(CustomerList));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _customerService.TDeleteAsync(id); TempData["Success"] = "Müşteri silindi."; }
        catch (Exception ex) { TempData["Error"] = ex.Message; }
        return RedirectToAction(nameof(CustomerList));
    }
}
