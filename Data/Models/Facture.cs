using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Facture : BaseCompanyIdEntity
{
    [Required]
    public long NumberFactures { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    [Required]
    public DateTime SaleDate { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    public string? Comment { get; set; }

    [Required]
    [ForeignKey(nameof(UserCompanyId))]
    public virtual Company UserCompany { get; set; }

    public Guid CompanyId { get; set; }

    [Required]
    [ForeignKey(nameof(CompanyId))]
    public virtual Company Company { get; set; }

    [Required]
    public virtual ICollection<FactureDetail> FactureDetails { get; set; }

    [Required]
    public virtual ICollection<Payment> Payments { get; set; }

    [Required]
    public virtual ICollection<PdfFile> PdfFiles { get; set; }
}
