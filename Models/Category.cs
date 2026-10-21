using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    [StringLength(100)]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
