using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Controllers;

public class InvoiceController : Controller
{
    // GET: /Invoice/Create
    [HttpGet]
    public IActionResult Create()
    {
        var vm = new InvoiceCreateVm
        {
            EmployeeId = SeedData.Employees.FirstOrDefault(e => e.IsActive)?.Id ?? 0
        };
        LoadSelectLists();
        return View(vm);
    }

    // POST: /Invoice/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(InvoiceCreateVm vm)
    {
        // 1) Validate từng dòng: sản phẩm tồn tại, đang bán, đủ tồn kho
        for (int i = 0; i < vm.Lines.Count; i++)
        {
            var line = vm.Lines[i];
            var product = SeedData.Products.FirstOrDefault(p => p.Id == line.ProductId);

            if (product is null)
            {
                ModelState.AddModelError($"Lines[{i}].ProductId", "Sản phẩm không tồn tại.");
                continue;
            }
            if (!product.IsActive)
            {
                ModelState.AddModelError($"Lines[{i}].ProductId",
                    $"Sản phẩm \"{product.Name}\" đã ngừng bán.");
            }
            if (line.Quantity > product.StockQuantity)
            {
                ModelState.AddModelError($"Lines[{i}].Quantity",
                    $"Chỉ còn {product.StockQuantity} sản phẩm trong kho.");
            }

            // Server-authoritative price — không tin giá gửi lên từ client
            line.UnitPrice = product.Price;
        }

        if (!ModelState.IsValid)
        {
            LoadSelectLists();
            return View(vm);
        }

        // 2) Tạo Invoice mới
        int newId = SeedData.Invoices.Any() ? SeedData.Invoices.Max(i => i.Id) + 1 : 1;
        var invoice = new Invoice
        {
            Id            = newId,
            CreatedAt     = DateTime.Now,
            EmployeeId    = vm.EmployeeId,
            CustomerName  = vm.CustomerName.Trim(),
            CustomerPhone = vm.CustomerPhone?.Trim(),
            TotalAmount   = vm.Lines.Sum(l => l.Quantity * l.UnitPrice),
        };
        SeedData.Invoices.Add(invoice);

        // 3) Tạo InvoiceDetail + trừ tồn kho
        int detailId = SeedData.InvoiceDetails.Any()
            ? SeedData.InvoiceDetails.Max(d => d.Id) + 1 : 1;

        foreach (var line in vm.Lines)
        {
            SeedData.InvoiceDetails.Add(new InvoiceDetail
            {
                Id        = detailId++,
                InvoiceId = invoice.Id,
                ProductId = line.ProductId,
                Quantity  = line.Quantity,
                UnitPrice = line.UnitPrice,
            });

            var product = SeedData.Products.First(p => p.Id == line.ProductId);
            product.StockQuantity -= line.Quantity;
        }

        TempData["Success"] = $"Đã tạo hoá đơn #{invoice.Id} cho khách hàng {invoice.CustomerName}.";
        return RedirectToAction(nameof(HomeController.InvoiceDetail), "Home",
            new { invoiceId = invoice.Id });
    }

    private void LoadSelectLists()
    {
        ViewBag.Employees = SeedData.Employees.Where(e => e.IsActive).ToList();
        ViewBag.Products  = SeedData.Products.Where(p => p.IsActive && p.StockQuantity > 0).ToList();
    }
}
