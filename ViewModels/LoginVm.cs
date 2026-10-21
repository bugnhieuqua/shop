using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels;

public class LoginVm
{
    [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
    [Display(Name = "Tài khoản")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;
}
