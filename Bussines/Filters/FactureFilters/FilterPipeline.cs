using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;
using Data.Models;

namespace Bussines.Filters.FactureFilters;
public class FilterPipeline : IFactureFilterPipeline
{
    private readonly List<IFacturePipelineGetHandler> _handlers;

    public FilterPipeline()
    {
        _handlers = new List<IFacturePipelineGetHandler>();
    }

    public void AddHandler(IFacturePipelineGetHandler handler)
    {
        _handlers.Add(handler);
    }

    public IQueryable<Facture> Execute(FactureFilterContext context)
    {
        foreach (var handler in _handlers)
        {
            context = handler.Handle(context);
        }

        return context.Query;
    }
}
