using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;

public class FactureNumberFilterHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        if (context.Criteria.FactureNumber.HasValue)
        {
            context.Query = context.Query.Where(f => f.NumberFactures == context.Criteria.FactureNumber.Value);
        }

        return context;
    }
}
