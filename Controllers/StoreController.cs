using Microsoft.AspNetCore.Mvc;
using Shop.Data;

namespace Shop.Controllers;

public class StoreController : Controller
{
    // Trang chủ cửa hàng dành cho khách hàng
    public IActionResult Index()
    {
        // Chỉ lấy sản phẩm đang bán và còn hàng trong kho
        var products = SeedData.Products
            .Where(p => p.IsActive && p.StockQuantity > 0)
            .ToList();

        ViewBag.Categories = SeedData.Categories;
        return View(products);
    }

    // Xem chi tiết sản phẩm
    public IActionResult Detail(int id)
    {
        var product = SeedData.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        var category = SeedData.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        ViewBag.CategoryName = category?.Name ?? "Khác";

        return View(product);
    }
}
