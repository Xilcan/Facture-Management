using Bussines.Dto.Product;

namespace Bussines.Dto.ProductCategory;
public class FullProductCategoryGet
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public virtual ICollection<BriefProductGet> Products { get; set; }
}