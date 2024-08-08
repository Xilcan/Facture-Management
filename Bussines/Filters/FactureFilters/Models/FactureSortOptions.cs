using System.Text.Json.Serialization;
using Interface.FiltersInterface.FactureFilterInterface;

namespace Bussines.Filters.FactureFilters.Models;
public class FactureSortOptions
{
    public FactureSortOptions()
    {
        var defaultOption = new string[] { "DateDesc" };
        Sort = defaultOption;
    }

    [JsonPropertyName("sort")]
    [SortingFactureValidation]
    public string[]? Sort { get; set; }
}
