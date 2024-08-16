using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Interfaces;

public interface IFacturePipelineGetHandler
{
    FactureFilterContext Handle(FactureFilterContext context);
}
