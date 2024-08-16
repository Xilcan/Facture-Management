using Data.Models;

namespace Bussines.Dto.Product;

public class BriefProductGet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Vat { get; set; }

    public TransactionTypes TransactionType { get; set; }

    public decimal Price { get; set; } = decimal.Zero;
}
