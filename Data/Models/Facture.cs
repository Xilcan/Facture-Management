﻿using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Facture
{
    [Key]
    [Required]
    public Guid Id { get; set; }

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

    public Guid UserCompanyId { get; set; }

    [Required]
    public virtual Company UserCompany { get; set; }

    public Guid CompanyId { get; set; }

    [Required]
    public virtual Company Company { get; set; }

    [Required]
    public virtual ICollection<FactureDetail> FactureDetails { get; set; }

    [Required]
    public virtual ICollection<Payment> Payments { get; set; }

    [Required]
    public virtual ICollection<PdfFile> PdfFiles { get; set; }
}
