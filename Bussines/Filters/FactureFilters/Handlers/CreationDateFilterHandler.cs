using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;

public class CreationDateFilterHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        if (context.Criteria.CreationDateStart.HasValue)
        {
            context.Query = context.Query.Where(f => f.CreationDate >= context.Criteria.CreationDateStart.Value);
        }

        if (context.Criteria.CreationDateEnd.HasValue)
        {
            context.Query = context.Query.Where(f => f.CreationDate <= context.Criteria.CreationDateEnd.Value);
        }

        return context;
    }
}
