using Bussines.Dto.ProductCategory;
using Data.Models;

namespace Bussines.Dto.Product;

public class FullProductGet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public TransactionTypes TransactionType { get; set; }

    public decimal Price { get; set; } = decimal.Zero;

    public decimal Vat { get; set; }

    public BriefProductCategoryGet Category { get; set; }
}
