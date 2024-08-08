using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussines.Dto.FactureDetails;
public class FactureDetailsPost
{
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
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
    public string Comment { get; set; } = string.Empty;

    [Required]
    public Guid ProductId { get; set; }
}
