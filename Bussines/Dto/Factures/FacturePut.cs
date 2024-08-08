using System.ComponentModel.DataAnnotations;
using Bussines.Dto.FactureDetails;
using Bussines.Dto.Payment;

namespace Bussines.Dto.Factures;
public class FacturePut
{
    [Required(ErrorMessage = "Id is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "NumberFactures is required.")]
    public long NumberFactures { get; set; }

    [Required(ErrorMessage = "Name is required.")]
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
    public ICollection<FactureDetailsPut> FactureDetails { get; set; }

    [Required(ErrorMessage = "Payments are required.")]
    public ICollection<PaymentGetPut> Payments { get; set; }
}
