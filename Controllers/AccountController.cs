using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.ViewModels;

namespace Shop.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        // Tìm nhân viên dựa vào UserId trong SeedData
        var employee = SeedData.Employees.FirstOrDefault(e =>
            e.UserId != null &&
            e.UserId.Equals(vm.UserId, StringComparison.OrdinalIgnoreCase));

        // Kiểm tra tồn tại, đang hoạt động và mật khẩu (Mock pass: 123456)
        if (employee == null || !employee.IsActive || vm.Password != "123456")
        {
            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng, hoặc nhân viên đã nghỉ việc.");
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }

        // Tạo Claims danh tính người dùng
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Name, employee.FullName),
            new Claim("UserId", employee.UserId ?? string.Empty),
            new Claim(ClaimTypes.Role, "Employee")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        TempData["Success"] = $"Chào mừng {employee.FullName} trở lại!";

        // Nếu có ReturnUrl hợp lệ thì quay lại trang yêu cầu trước đó
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Success"] = "Đã đăng xuất khỏi tài khoản nhân viên.";
        return RedirectToAction("Index", "Store");
    }
}
