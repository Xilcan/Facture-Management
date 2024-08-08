using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussines.Dto.Payment;
public class PaymentGetPut
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Method { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDeadline { get; set; }

    public DateTime? PaymantDate { get; set; }
}
