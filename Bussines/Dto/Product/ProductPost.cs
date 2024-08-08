using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models;

namespace Bussines.Dto.Product;
public class ProductPost
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public TransactionTypes TransactionType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Price { get; set; } = decimal.Zero;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 100)]
    public decimal Vat { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
}
