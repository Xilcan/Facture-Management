using Bussines.Dto.Factures;
using Bussines.Filters.FactureFilters.Models;
using Interface.FiltersInterface.PagingFilterInterface;

namespace Bussines.Services.Interfaces;

public interface IFactureService
{
    public Task<FullFactureGet> GetFactureByIdAsync(Guid id, Guid userCompanyId);

    public Task<IEnumerable<BriefFacturesGet>> GetFacturesAsync(FactureFilterCriteria factureSearchModel, FactureSortOptions factureSortOptions, PagingDtoOption pagingDtoOption, Guid userCompanyId);

    public Task AddAsync(FacturePost facture, Guid userCompanyId);

    public Task UpdateAsync(FacturePut facture, Guid userCompanyId);

    public Task DeleteAsync(Guid factureId, Guid userCompanyId);
}
