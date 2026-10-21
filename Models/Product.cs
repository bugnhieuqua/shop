using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Đơn giá")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Tồn kho")]
    public int StockQuantity { get; set; }

    [StringLength(255)]
    [Display(Name = "Ảnh")]
    public string? ImageFileName { get; set; }

    [Display(Name = "Đang bán")]
    public bool IsActive { get; set; } = true;

    public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
