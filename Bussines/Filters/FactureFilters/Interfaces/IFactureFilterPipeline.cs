using Bussines.Filters.FactureFilters.Models;
using Data.Models;

namespace Bussines.Filters.FactureFilters.Interfaces;

public interface IFactureFilterPipeline
{
    void AddHandler(IFacturePipelineGetHandler handler);

    IQueryable<Facture> Execute(FactureFilterContext context);
}
