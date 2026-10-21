using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shop.ViewModels;

public class ProductCreateVm
{
    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(150, ErrorMessage = "Tên sản phẩm tối đa 150 ký tự")]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục")]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
    [Display(Name = "Đơn giá (VNĐ)")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không hợp lệ")]
    [Display(Name = "Số lượng tồn kho")]
    public int StockQuantity { get; set; }

    [Display(Name = "Hình ảnh")]
    public IFormFile? ImageFile { get; set; }

    [Display(Name = "Mở bán sản phẩm này")]
    public bool IsActive { get; set; } = true;
}
