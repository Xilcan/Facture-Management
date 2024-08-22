using Bussines.Dto.Product;

namespace Bussines.Services.Interfaces;

public interface IProductService
{
    public Task<FullProductGet> GetByIdAsync(Guid id, Guid userCompanyId);

    public Task<IEnumerable<BriefProductGet>> GetAllAsync(Guid? categoryId, Guid userCompanyId);

    public Task AddAsync(ProductPost product, Guid userCompanyId);

    public Task UpdateAsync(ProductPut product, Guid userCompanyId);

    public Task DeleteAsync(Guid id, Guid userCompanyId);
}
