using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers;

[Authorize]
public class HomeController : Controller
{
    // Trang chủ: dashboard tổng hợp
    public IActionResult Index()
    {
        ViewBag.ProductCount  = SeedData.Products.Count;
        ViewBag.EmployeeCount = SeedData.Employees.Count(p => p.IsActive);
        ViewBag.InvoiceCount  = SeedData.Invoices.Count;
        ViewBag.Revenue       = SeedData.Invoices.Sum(i => i.TotalAmount);

        ViewBag.LatestInvoices = SeedData.Invoices
            .OrderByDescending(i => i.CreatedAt)
            .Take(5)
            .ToList();

        return View();
    }

    // Danh mục
    public IActionResult Category()
    {
        return View(SeedData.Categories.ToList());
    }

    // Sản phẩm trong kho
    public IActionResult Product()
    {
        ViewBag.Categories = SeedData.Categories;
        return View(SeedData.Products.ToList());
    }

    // Nhân viên
    public IActionResult Employee()
    {
        return View(SeedData.Employees.ToList());
    }

    // Hóa đơn
    public IActionResult Invoice()
    {
        return View(SeedData.Invoices.OrderByDescending(i => i.CreatedAt).ToList());
    }

    // Chi tiết hóa đơn (lọc theo invoiceId nếu có)
    public IActionResult InvoiceDetail(int? invoiceId)
    {
        ViewBag.Invoices = SeedData.Invoices;
        ViewBag.SelectedInvoiceId = invoiceId;

        var list = invoiceId is null
            ? SeedData.InvoiceDetails.ToList()
            : SeedData.InvoiceDetails.Where(d => d.InvoiceId == invoiceId).ToList();

        return View(list);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
