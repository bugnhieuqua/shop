using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    // ================== THÊM NHÂN VIÊN ==================
    [HttpGet]
    public IActionResult Create()
    {
        return View(new EmployeeCreateVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EmployeeCreateVm vm)
    {
        // Business validation: UserId trùng
        if (SeedData.Employees.Any(e =>
                e.UserId != null && e.UserId.Equals(vm.UserId, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(nameof(vm.UserId), "Tài khoản này đã được sử dụng.");
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var entity = new Employee
        {
            Id       = SeedData.Employees.Any() ? SeedData.Employees.Max(e => e.Id) + 1 : 1,
            FullName = vm.FullName.Trim(),
            Phone    = vm.Phone?.Trim(),
            Email    = vm.Email.Trim(),
            UserId   = vm.UserId.Trim(),
            IsActive = vm.IsActive,
        };
        SeedData.Employees.Add(entity);

        TempData["Success"] = $"Đã thêm nhân viên \"{entity.FullName}\".";
        return RedirectToAction(nameof(HomeController.Employee), "Home");
    }

    // ================== SỬA NHÂN VIÊN ==================
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var emp = SeedData.Employees.FirstOrDefault(e => e.Id == id);
        if (emp == null) return NotFound();

        var vm = new EmployeeCreateVm
        {
            FullName = emp.FullName,
            Phone = emp.Phone,
            Email = emp.Email ?? string.Empty,
            UserId = emp.UserId ?? string.Empty,
            IsActive = emp.IsActive
        };

        ViewBag.EmployeeId = id;
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EmployeeCreateVm vm)
    {
        var emp = SeedData.Employees.FirstOrDefault(e => e.Id == id);
        if (emp == null) return NotFound();

        // Validate: UserId không được trùng với người khác (trừ chính mình)
        if (SeedData.Employees.Any(e => e.Id != id &&
            e.UserId != null && e.UserId.Equals(vm.UserId, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError(nameof(vm.UserId), "Tài khoản này đã được sử dụng bởi nhân viên khác.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.EmployeeId = id;
            return View(vm);
        }

        // Cập nhật thông tin
        emp.FullName = vm.FullName.Trim();
        emp.Phone = vm.Phone?.Trim();
        emp.Email = vm.Email.Trim();
        emp.UserId = vm.UserId.Trim();
        emp.IsActive = vm.IsActive;

        TempData["Success"] = $"Đã cập nhật thông tin nhân viên \"{emp.FullName}\".";
        return RedirectToAction(nameof(HomeController.Employee), "Home");
    }
}
