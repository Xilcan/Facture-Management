using Bussines.Dto.Product;

namespace Bussines.Services.Interfaces;
public interface IProductService
{
    public Task<FullProductGet> GetByIdAsync(Guid id);

    public Task<IEnumerable<BriefProductGet>> GetAllAsync(Guid? categoryId);

    public Task AddAsync(ProductPost product);

    public Task UpdateAsync(ProductPut product);

    public Task DeleteAsync(Guid id);
}
