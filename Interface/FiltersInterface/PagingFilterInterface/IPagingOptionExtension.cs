namespace Interface.FiltersInterface.PagingFilterInterface;
public interface IPagingOptionExtension
{
    IEnumerable<string> GetStringOptions();

    PagingOption GetPagingOption(PagingDtoOption pagingDtoOption);
}
