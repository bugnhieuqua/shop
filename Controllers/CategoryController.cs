using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers;

public class CategoryController : Controller
{
    // GET: /Category/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Category());
    }

    // POST: /Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Sinh Id mới tự động
        model.Id = SeedData.Categories.Any()
            ? SeedData.Categories.Max(c => c.Id) + 1
            : 1;

        SeedData.Categories.Add(model);

        TempData["Success"] = $"Đã thêm danh mục \"{model.Name}\".";
        return RedirectToAction(nameof(HomeController.Category), "Home");
    }
}
