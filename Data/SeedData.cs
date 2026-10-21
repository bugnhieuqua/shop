using Shop.Models;

namespace Shop.Data;

public static class SeedData
{
    public static List<Category> Categories = new()
    {
        new() { Id = 1, Name = "Điện thoại", Description = "Smartphone chính hãng" },
        new() { Id = 2, Name = "Laptop",    Description = "Laptop văn phòng & gaming" },
        new() { Id = 3, Name = "Phụ kiện",  Description = "Tai nghe, sạc, ốp lưng" },
    };

    public static List<Product> Products = new()
    {
        new() { Id = 1, Name = "iPhone 15 Pro",   CategoryId = 1, Price = 28_990_000, StockQuantity = 12, ImageFileName = "iphone15pro.png", IsActive = true },
        new() { Id = 2, Name = "Samsung S24",     CategoryId = 1, Price = 22_490_000, StockQuantity = 3,  ImageFileName = "s24.png",        IsActive = true },
        new() { Id = 3, Name = "Dell XPS 13",     CategoryId = 2, Price = 32_000_000, StockQuantity = 0,  ImageFileName = "xps13.png",      IsActive = false },
        new() { Id = 4, Name = "Tai nghe AirPods",CategoryId = 3, Price =  4_990_000, StockQuantity = 25, ImageFileName = "airpods.png",    IsActive = true },
    };

    public static List<Employee> Employees = new()
    {
        new() { Id = 1, FullName = "Nguyễn Văn An",  Phone = "0901234567", Email = "an@shop.vn",    UserId = "u-an",    IsActive = true },
        new() { Id = 2, FullName = "Trần Thị Bình",  Phone = "0912345678", Email = "binh@shop.vn",  UserId = "u-binh",  IsActive = true },
        new() { Id = 3, FullName = "Lê Hoàng Cường", Phone = "0987654321", Email = "cuong@shop.vn", UserId = "u-cuong", IsActive = false },
    };

    public static List<Invoice> Invoices = new()
    {
        new() { Id = 1, CreatedAt = DateTime.Today.AddDays(-2), EmployeeId = 1, CustomerName = "Phạm Minh", CustomerPhone = "0933111222", TotalAmount = 33_980_000 },
        new() { Id = 2, CreatedAt = DateTime.Today.AddDays(-1), EmployeeId = 2, CustomerName = "Đỗ Thu Hà", CustomerPhone = "0944555666", TotalAmount = 22_490_000 },
    };

    public static List<InvoiceDetail> InvoiceDetails = new()
    {
        new() { Id = 1, InvoiceId = 1, ProductId = 1, Quantity = 1, UnitPrice = 28_990_000 },
        new() { Id = 2, InvoiceId = 1, ProductId = 4, Quantity = 1, UnitPrice =  4_990_000 },
        new() { Id = 3, InvoiceId = 2, ProductId = 2, Quantity = 1, UnitPrice = 22_490_000 },
    };
}
