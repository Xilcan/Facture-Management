using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Company
{
    public Company()
    {
        var factures = new List<Facture>();
        Factures = factures;
    }

    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public long NIP { get; set; }

    public virtual ICollection<Facture> Factures { get; set; }

    public virtual ICollection<Facture> UserFactures { get; set; }

    [Required]
    public virtual CompanyAddress Address { get; set; }
}
