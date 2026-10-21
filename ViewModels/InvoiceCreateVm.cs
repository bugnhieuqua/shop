using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels;

public class InvoiceCreateVm
{
    [Required(ErrorMessage = "Vui lòng chọn nhân viên lập hoá đơn")]
    [Display(Name = "Nhân viên")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Tên khách hàng không được để trống")]
    [StringLength(100, ErrorMessage = "Tên khách hàng tối đa 100 ký tự")]
    [Display(Name = "Khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    [Display(Name = "SĐT khách")]
    public string? CustomerPhone { get; set; }

    [MinLength(1, ErrorMessage = "Hoá đơn phải có ít nhất 1 sản phẩm")]
    public List<InvoiceLineVm> Lines { get; set; } = new() { new InvoiceLineVm() };
}

public class InvoiceLineVm
{
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn sản phẩm")]
    [Display(Name = "Sản phẩm")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ≥ 1")]
    [Display(Name = "Số lượng")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }
}
