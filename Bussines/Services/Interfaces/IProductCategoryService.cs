using Bussines.Dto.ProductCategory;

namespace Bussines.Services.Interfaces;
public interface IProductCategoryService
{
    public Task<FullProductCategoryGet> GetByIdAsync(Guid id);

    public Task<IEnumerable<BriefProductCategoryGet>> GetAllAsync();

    public Task AddAsync(ProductCategoryPost productCategory);

    public Task UpdateAsync(BriefProductCategoryGet productCategory);

    public Task DeleteAsync(Guid id);
}
