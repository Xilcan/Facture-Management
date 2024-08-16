using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;

public class CustomerNameFilterHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        if (!string.IsNullOrEmpty(context.Criteria.CustomerName))
        {
            context.Query = context.Query.Where(f => f.Company.Name.Contains(context.Criteria.CustomerName));
        }

        return context;
    }
}
