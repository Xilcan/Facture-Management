using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class FactureDetail
{
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 9999999999999999.99)]
    public decimal UnitPriceNetto { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 9999999999999999.99)]
    public decimal UnitPriceBrutto { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 100)]
    public decimal Vat { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Comment { get; set; }

    [Required]
    public Guid FactureId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Facture Factures { get; set; }

    [Required]
    public Product Products { get; set; }
}
