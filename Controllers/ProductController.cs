using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly IWebHostEnvironment _env;

    public ProductController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // ================== THÊM SẢN PHẨM ==================
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = SeedData.Categories;
        return View(new ProductCreateVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = SeedData.Categories;
            return View(vm);
        }

        // Xử lý upload ảnh
        string? fileName = null;
        if (vm.ImageFile != null && vm.ImageFile.Length > 0)
        {
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
            var uploadPath = Path.Combine(_env.WebRootPath, "images", "products");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.ImageFile.CopyToAsync(stream);
            }
        }

        // Lưu vào danh sách dữ liệu (SeedData)
        var product = new Product
        {
            Id = SeedData.Products.Any() ? SeedData.Products.Max(p => p.Id) + 1 : 1,
            Name = vm.Name.Trim(),
            CategoryId = vm.CategoryId,
            Price = vm.Price,
            StockQuantity = vm.StockQuantity,
            ImageFileName = fileName ?? "placeholder.png",
            IsActive = vm.IsActive
        };
        SeedData.Products.Add(product);

        TempData["Success"] = $"Đã thêm sản phẩm \"{product.Name}\".";
        return RedirectToAction(nameof(HomeController.Product), "Home");
    }

    // ================== SỬA SẢN PHẨM ==================
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = SeedData.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        ViewBag.Categories = SeedData.Categories;
        ViewBag.ExistingImage = product.ImageFileName;
        ViewBag.ProductId = product.Id;

        var vm = new ProductCreateVm
        {
            Name = product.Name,
            CategoryId = product.CategoryId,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductCreateVm vm)
    {
        var product = SeedData.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = SeedData.Categories;
            ViewBag.ExistingImage = product.ImageFileName;
            ViewBag.ProductId = id;
            return View(vm);
        }

        // Xử lý upload ảnh mới nếu người dùng chọn
        if (vm.ImageFile != null && vm.ImageFile.Length > 0)
        {
            // Xoá ảnh cũ (nếu không phải là placeholder hoặc ảnh mẫu mặc định)
            var defaultImages = new[] { "iphone15pro.png", "s24.png", "xps13.png", "airpods.png", "placeholder.png" };
            if (!string.IsNullOrEmpty(product.ImageFileName) && !defaultImages.Contains(product.ImageFileName))
            {
                var oldPath = Path.Combine(_env.WebRootPath, "images", "products", product.ImageFileName);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // Lưu ảnh mới
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
            var uploadPath = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.ImageFile.CopyToAsync(stream);
            }
            product.ImageFileName = fileName;
        }

        // Cập nhật thông tin sản phẩm
        product.Name = vm.Name.Trim();
        product.CategoryId = vm.CategoryId;
        product.Price = vm.Price;
        product.StockQuantity = vm.StockQuantity;
        product.IsActive = vm.IsActive;

        TempData["Success"] = $"Đã cập nhật sản phẩm \"{product.Name}\".";
        return RedirectToAction(nameof(HomeController.Product), "Home");
    }
}
