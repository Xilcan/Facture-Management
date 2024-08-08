using Bussines.Dto.Product;

namespace Bussines.Dto.FactureDetails;
public class FactureDetailsGet
{
    public Guid Id { get; set; }

    public decimal UnitPriceNetto { get; set; }

    public decimal UnitPriceBrutto { get; set; }

    public decimal Vat { get; set; }

    public int Quantity { get; set; }

    public string Name { get; set; }

    public string Comment { get; set; } = string.Empty;

    public FullProductGet Product { get; set; }
}
