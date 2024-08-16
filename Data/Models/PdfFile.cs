using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class PdfFile
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid FactureId { get; set; }

    public byte[] Data { get; set; }

    [ForeignKey(nameof(FactureId))]
    public virtual Facture Facture { get; set; }
}
