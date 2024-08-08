using System.ComponentModel.DataAnnotations;

namespace Bussines.Dto.Factures;
public class BriefFacturesGet
{
    public Guid Id { get; set; }

    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "NumberFactures must be 1 or more.")]
    public long NumberFactures { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string CustomerName { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime SaleDate { get; set; } = DateTime.Now;

    public DateTime PaymentDate { get; set; } = DateTime.Now;
}
