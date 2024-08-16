using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;

namespace Bussines.Filters.FactureFilters.Handlers;

public class PaymentDateFilterHandler : IFacturePipelineGetHandler
{
    FactureFilterContext IFacturePipelineGetHandler.Handle(FactureFilterContext context)
    {
        if (context.Criteria.PaymentDateStart != null)
        {
            context.Query = context.Query.Where(q => q.PaymentDate >= context.Criteria.PaymentDateStart);
        }

        if (context.Criteria.PaymentDateEnd != null)
        {
            context.Query = context.Query.Where(q => q.PaymentDate <= context.Criteria.PaymentDateEnd);
        }

        return context;
    }
}
