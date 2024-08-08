using Data.Models;

namespace Data.Repositories.Interfaces;
public interface IProductRepository
{
    public Task<Product> GetByIdAsync(Guid id);

    public Task<IEnumerable<Product>> GetAllAsync();

    public Task AddAsync(Product product);

    public void Update(Product product);

    public void Delete(Product product);

    public Task<bool> ExistsByNameAsync(string name);

    public Task<bool> ExistsByIdAsync(Guid id);
}
