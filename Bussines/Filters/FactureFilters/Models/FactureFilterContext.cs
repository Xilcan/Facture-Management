using Data.Models;
using Interface.FiltersInterface.PagingFilterInterface;

namespace Bussines.Filters.FactureFilters.Models;

public class FactureFilterContext
{
    public IQueryable<Facture> Query { get; set; }

    public PagingOption PagingOption { get; set; }

    public FactureFilterCriteria Criteria { get; set; }

    public FactureSortOptions SortingOption { get; set; }
}
