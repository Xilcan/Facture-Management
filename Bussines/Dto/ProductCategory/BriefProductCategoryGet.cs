namespace Bussines.Dto.ProductCategory;
public class BriefProductCategoryGet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;
}
