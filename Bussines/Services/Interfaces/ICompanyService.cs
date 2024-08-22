using Bussines.Dto.Company;

namespace Bussines.Services.Interfaces;

public interface ICompanyService
{
    public Task<FullCompanyGet> GetByIdAsync(Guid id, Guid userCompanyId);

    public Task<IEnumerable<BriefCompanyGet>> GetAllAsync(Guid userCompanyId);

    public Task AddAsync(CompanyPost company, Guid userCompanyId);

    public Task UpdateAsync(FullCompanyPut company, Guid userCompanyId);

    public Task DeleteAsync(Guid id, Guid userCompanyId);
}
