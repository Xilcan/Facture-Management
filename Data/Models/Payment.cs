﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Payment
{
    public Guid Id { get; set; }

    public string Method { get; set; } = "Cash";

    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, 9999999999999999.99)]
    public decimal Amount { get; set; }

    public DateTime PaymentDeadline { get; set; }

    public DateTime? PaymantDate { get; set; }

    [Required]
    public Guid FactureId { get; set; }

    public virtual Facture Facture { get; set; }
}
