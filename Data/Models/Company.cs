using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Company : BaseEntity
{
    public Company()
    {
        var factures = new List<Facture>();
        Factures = factures;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public long NIP { get; set; }

    public Guid? UserCompanyId { get; set; }

    public virtual ICollection<Facture> Factures { get; set; }

    public virtual ICollection<Facture> UserFactures { get; set; }

    [Required]
    public virtual CompanyAddress Address { get; set; }
}
