using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels;

public class EmployeeCreateVm
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    [Display(Name = "Điện thoại")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(150)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tài khoản đăng nhập không được để trống")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tài khoản từ 3–50 ký tự")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Chỉ dùng chữ, số, dấu chấm, gạch dưới, gạch ngang")]
    [Display(Name = "Tài khoản")]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Đang làm việc")]
    public bool IsActive { get; set; } = true;
}
