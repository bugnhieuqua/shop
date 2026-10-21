using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class InvoiceDetail
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Range(1, int.MaxValue)]
    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }

    // Computed — không map DB
    [NotMapped]
    public decimal LineTotal => Quantity * UnitPrice;
}
