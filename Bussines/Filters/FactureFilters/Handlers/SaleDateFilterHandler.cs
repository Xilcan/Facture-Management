using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;
public class SaleDateFilterHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        if (context.Criteria.SaleDateStart != null)
        {
            context.Query = context.Query.Where(q => q.SaleDate >= context.Criteria.SaleDateStart);
        }

        if (context.Criteria.SaleDateEnd != null)
        {
            context.Query = context.Query.Where(q => q.SaleDate <= context.Criteria.SaleDateEnd);
        }

        return context;
    }
}
