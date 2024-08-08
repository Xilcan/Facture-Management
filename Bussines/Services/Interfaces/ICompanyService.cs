using Bussines.Dto.Company;

namespace Bussines.Services.Interfaces;

public interface ICompanyService
{
    public Task<FullCompanyGet> GetByIdAsync(Guid id);

    public Task<IEnumerable<BriefCompanyGet>> GetAllAsync();

    public Task AddAsync(CompanyPost company);

    public Task UpdateAsync(FullCompanyPut company);

    public Task DeleteAsync(Guid id);
}
