namespace Data.Models;

public class ProductCategory : BaseCompanyIdEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public virtual ICollection<Product> Products { get; set; }
}
