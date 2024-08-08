namespace Bussines.Filters.FactureFilters.Models;
public class FactureFilterCriteria
{
    public string? FactureName { get; set; }

    public long? FactureNumber { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? CreationDateStart { get; set; }

    public DateTime? SaleDateStart { get; set; }

    public DateTime? PaymentDateStart { get; set; }

    public DateTime? CreationDateEnd { get; set; }

    public DateTime? SaleDateEnd { get; set; }

    public DateTime? PaymentDateEnd { get; set; }
}
