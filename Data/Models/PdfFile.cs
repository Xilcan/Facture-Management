namespace Data.Models;
public class PdfFile
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid FactureId { get; set; }

    public byte[] Data { get; set; }

    public virtual Facture Facture { get; set; }
}
