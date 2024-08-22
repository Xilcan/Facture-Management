using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<Product> GetByIdAsync(Guid id, Guid userCompanyId);

    public Task<IEnumerable<Product>> GetAllAsync(Guid userCompanyId);

    public Task AddAsync(Product product);

    public void Update(Product product);

    public void Delete(Product product);

    public Task<bool> ExistsByNameAsync(string name, Guid userCompanyId);

    public Task<bool> ExistsByIdAsync(Guid id, Guid userCompanyId);
}
