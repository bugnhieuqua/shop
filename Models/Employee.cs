using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    [Display(Name = "Điện thoại")]
    public string? Phone { get; set; }

    [EmailAddress, StringLength(150)]
    public string? Email { get; set; }

    // Liên kết tới tài khoản đăng nhập (Identity / bảng Users)
    [Display(Name = "Tài khoản")]
    public string? UserId { get; set; }

    [Display(Name = "Đang làm việc")]
    public bool IsActive { get; set; } = true;

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
