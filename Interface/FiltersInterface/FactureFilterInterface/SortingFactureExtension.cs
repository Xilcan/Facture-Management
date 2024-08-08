namespace Interface.FiltersInterface.FactureFilterInterface;
public class SortingFactureExtension : ISortingFactureExtension
{
    public IEnumerable<string> GetSortingFactureOptions()
    {
        return Enum.GetNames<EnumSortingFacture>();
    }

    public Dictionary<EnumSortingFacture, (string PropertyName, bool IsAscending)> SortOptionToProperty()
    {
        return new()
    {
        { EnumSortingFacture.DateAsc, ("CreationDate", true) },
        { EnumSortingFacture.DateDesc, ("CreationDate", false) },
        { EnumSortingFacture.Name, ("Name", true) },
        { EnumSortingFacture.FactureNumber, ("NumberFactures", false) },
        { EnumSortingFacture.CompanyName, ("Company.Name", true) },
    };
    }
}
