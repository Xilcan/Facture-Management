using System.ComponentModel.DataAnnotations;
using Bussines.Dto.Company;
using Bussines.Dto.FactureDetails;
using Bussines.Dto.Payment;

namespace Bussines.Dto.Factures;

public class FullFactureGet
{
    public Guid Id { get; set; }

    [Required]
    public long NumberFactures { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string City { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime SaleDate { get; set; } = DateTime.Now;

    public DateTime PaymentDate { get; set; } = DateTime.Now;

    public string? Comment { get; set; }

    [Required]
    public FullCompanyGet UserCompany { get; set; }

    [Required]
    public FullCompanyGet Company { get; set; }

    [Required]
    public ICollection<FactureDetailsGet> FactureDetails { get; set; }

    [Required]
    public ICollection<PaymentGetPut> Payments { get; set; }

    [Required]
    public ICollection<PdfFileGet> PdfFiles { get; set; }
}
