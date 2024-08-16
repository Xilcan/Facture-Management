namespace Interface.FiltersInterface.PagingFilterInterface;

public class PagingOptionExtension : IPagingOptionExtension
{
    public IEnumerable<string> GetStringOptions() => new List<string> { "10", "20", "50", "100", "all" };

    public PagingOption GetPagingOption(PagingDtoOption pagingDtoOption)
    {
        if (!int.TryParse(pagingDtoOption.PageCount, out int limit))
        {
            limit = 0;
        }

        return new PagingOption
        {
            Page = pagingDtoOption.Page ?? 1,
            PageCount = limit,
        };
    }
}
