namespace Interface.FiltersInterface.FactureFilterInterface;

public interface ISortingFactureExtension
{
    IEnumerable<string> GetSortingFactureOptions();

    Dictionary<EnumSortingFacture, (string PropertyName, bool IsAscending)> SortOptionToProperty();
}
