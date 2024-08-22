using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ICompanyRepository
{
    public Task<Company> GetByIdAsync(Guid id, Guid? userCompanyId);

    public Task<IEnumerable<Company>> GetAllAsync(Guid userCompanyId);

    public Task AddAsync(Company company);

    public void Update(Company company);

    public void Delete(Company company);

    public Task<bool> ExistsByIdAsync(Guid id, Guid userCompanyId);

    public Task<bool> ExistsByNIPAsync(long nip, Guid userCompanyId);

    public void DeleteCompanyAdressesByRange(IEnumerable<CompanyAddress> companyAddresses);
}
