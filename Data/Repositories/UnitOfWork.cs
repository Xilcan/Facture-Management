using Data.Repositories.Interfaces;

namespace Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FacturesManagementContext _facturesManagementContext;

    public UnitOfWork(
        FacturesManagementContext facturesManagementContext,
        ICompanyRepository companyRepository,
        IFactureRepository factureRepository,
        IProductCategoryRepository productCategoryRepository,
        IProductRepository productRepository)
    {
        ArgumentNullException.ThrowIfNull(facturesManagementContext);
        ArgumentNullException.ThrowIfNull(companyRepository);
        ArgumentNullException.ThrowIfNull(productCategoryRepository);
        ArgumentNullException.ThrowIfNull(productRepository);
        ArgumentNullException.ThrowIfNull(factureRepository);

        _facturesManagementContext = facturesManagementContext;
        CompanyRepository = companyRepository;
        ProductCategoryRepository = productCategoryRepository;
        FactureRepository = factureRepository;
        ProductRepository = productRepository;
    }

    public ICompanyRepository CompanyRepository { get; private set; }

    public IFactureRepository FactureRepository { get; private set; }

    public IProductCategoryRepository ProductCategoryRepository { get; private set; }

    public IProductRepository ProductRepository { get; private set; }

    public async Task SaveAsync()
    {
        await _facturesManagementContext.SaveChangesAsync();
    }
}
