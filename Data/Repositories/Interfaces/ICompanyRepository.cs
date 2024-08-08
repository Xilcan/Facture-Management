using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ICompanyRepository
{
    public Task<Company> GetByIdAsync(Guid id);

    public Task<IEnumerable<Company>> GetAllAsync();

    public Task AddAsync(Company company);

    public void Update(Company company);

    public void Delete(Company company);

    public Task<bool> ExistsByIdAsync(Guid id);

    public Task<bool> ExistsByNIPAsync(long nip);

    public void DeleteCompanyAdressesByRange(IEnumerable<CompanyAddress> companyAddresses);
}
