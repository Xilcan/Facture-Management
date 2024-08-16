using Bussines.Dto.Factures;
using Bussines.Filters.FactureFilters.Models;
using Interface.FiltersInterface.PagingFilterInterface;

namespace Bussines.Services.Interfaces;

public interface IFactureService
{
    public Task<FullFactureGet> GetFactureByIdAsync(Guid id);

    public Task<IEnumerable<BriefFacturesGet>> GetFacturesAsync(FactureFilterCriteria factureSearchModel, FactureSortOptions factureSortOptions, PagingDtoOption pagingDtoOption);

    public Task AddAsync(FacturePost facture);

    public Task UpdateAsync(FacturePut facture);

    public Task DeleteAsync(Guid factureId);
}
