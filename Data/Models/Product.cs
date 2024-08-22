using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Product : BaseCompanyIdEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public TransactionTypes TransactionType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 9999999999999999.99)]
    public decimal Price { get; set; } = decimal.Zero;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 100)]
    public decimal Vat { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public virtual ICollection<FactureDetail> FactureDetails { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual ProductCategory Category { get; set; }
}
