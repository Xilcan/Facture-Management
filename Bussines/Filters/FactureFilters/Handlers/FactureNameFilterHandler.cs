using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;

public class FactureNameFilterHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        if (!string.IsNullOrEmpty(context.Criteria.FactureName))
        {
            context.Query = context.Query.Where(f => f.Name.Contains(context.Criteria.FactureName));
        }

        return context;
    }
}
