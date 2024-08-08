using Data.Models;

namespace Data.Repositories.Interfaces;
public interface IProductCategoryRepository
{
    public Task<ProductCategory> GetByIdAsync(Guid id);

    public Task<IEnumerable<ProductCategory>> GetAllAsync();

    public Task AddAsync(ProductCategory productCategory);

    public void Update(ProductCategory productCategory);

    public void Delete(ProductCategory productCategory);

    public Task<bool> ExistByNameAsync(string name);
}
