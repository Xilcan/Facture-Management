using Bussines.Dto.ProductCategory;

namespace Bussines.Services.Interfaces;

public interface IProductCategoryService
{
    public Task<FullProductCategoryGet> GetByIdAsync(Guid id, Guid userCompanyId);

    public Task<IEnumerable<BriefProductCategoryGet>> GetAllAsync(Guid userCompanyId);

    public Task AddAsync(ProductCategoryPost productCategory, Guid userCompanyId);

    public Task UpdateAsync(BriefProductCategoryGet productCategory, Guid userCompanyId);

    public Task DeleteAsync(Guid id, Guid userCompanyId);
}
