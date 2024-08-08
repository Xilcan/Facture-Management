using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;
public class FacturePagingHandler : IFacturePipelineGetHandler
{
    public FactureFilterContext Handle(FactureFilterContext context)
    {
        context.PagingOption.TotalPages = context.PagingOption.PageCount == 0 ? 1 : (int)Math.Ceiling((double)context.Query.Count() / context.PagingOption.PageCount);

        context.Query = context.Query.Skip((context.PagingOption.Page - 1) * context.PagingOption.PageCount);

        if (context.PagingOption.PageCount > 0)
        {
            context.Query = context.Query.Take(context.PagingOption.PageCount);
        }

        return context;
    }
}
