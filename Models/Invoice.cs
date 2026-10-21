using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class Invoice
{
    public int Id { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "Nhân viên")]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    [Display(Name = "SĐT khách")]
    public string? CustomerPhone { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount { get; set; }

    public ICollection<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
}
