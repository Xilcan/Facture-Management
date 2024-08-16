namespace Bussines.Dto;

public class PdfFileGet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public byte[] Data { get; set; }
}
