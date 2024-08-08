using System.Linq.Expressions;
using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;
using Data.Models;
using Interface.FiltersInterface.FactureFilterInterface;

namespace Bussines.Filters.FactureFilters.Handlers;
public class FactureSortingHandler : IFacturePipelineGetHandler
{
    private readonly ISortingFactureExtension _sortingFactureExtension;

    public FactureSortingHandler(ISortingFactureExtension sortingFactureExtension)
    {
        ArgumentNullException.ThrowIfNull(sortingFactureExtension);

        _sortingFactureExtension = sortingFactureExtension;
    }

    public FactureFilterContext Handle(FactureFilterContext context)
    {
        var sortMapping = _sortingFactureExtension.SortOptionToProperty();
        bool firstSortApplied = false;

        foreach (var sortOption in context.SortingOption.Sort)
        {
            if (Enum.TryParse(sortOption, out EnumSortingFacture enumValue) && sortMapping.TryGetValue(enumValue, out var sortInfo))
            {
                var param = Expression.Parameter(typeof(Facture), "f");
                var property = Expression.PropertyOrField(param, sortInfo.PropertyName);
                var lambda = Expression.Lambda(property, param);

                context.Query = CallOrderByOrThenBy(context.Query, firstSortApplied, sortInfo.IsAscending, property.Type, lambda);
                firstSortApplied = true;
            }
        }

        return context;
    }

    private static IQueryable<T> CallOrderByOrThenBy<T>(IQueryable<T> query, bool useThenBy, bool ascending, Type propertyType, LambdaExpression keySelector)
    {
        var methodName = useThenBy
            ? (ascending ? "ThenBy" : "ThenByDescending")
            : (ascending ? "OrderBy" : "OrderByDescending");

        var method = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(typeof(T), propertyType);

        return (IQueryable<T>)method.Invoke(null, new object[] { query, keySelector });
    }
}
