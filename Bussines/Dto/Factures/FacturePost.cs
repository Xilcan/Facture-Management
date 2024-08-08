using System.ComponentModel.DataAnnotations;
using Bussines.Dto.FactureDetails;
using Bussines.Dto.Payment;

namespace Bussines.Dto.Factures;
public class FacturePost
{
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "NumberFactures must be 1 or more.")]
    public long NumberFactures { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Name must be at least 1 sign")]
    public string Name { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "City is required.")]
    public string City { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "PaymentDate is required.")]
    public DateTime PaymentDate { get; set; }

    public string? Comment { get; set; }

    [Required(ErrorMessage = "UserCompanyId is required.")]
    public Guid UserCompanyId { get; set; }

    [Required(ErrorMessage = "CompanyId is required.")]
    public Guid CompanyId { get; set; }

    [Required(ErrorMessage = "FactureDetails are required.")]
    public ICollection<FactureDetailsPost> FactureDetails { get; set; }

    [Required(ErrorMessage = "Payments are required.")]
    public ICollection<PaymentPost> Payments { get; set; }
}
