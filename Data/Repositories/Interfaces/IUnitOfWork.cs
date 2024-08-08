namespace Data.Repositories.Interfaces;
public interface IUnitOfWork
{
    ICompanyRepository CompanyRepository { get; }

    IFactureRepository FactureRepository { get; }

    IProductCategoryRepository ProductCategoryRepository { get; }

    IProductRepository ProductRepository { get; }

    Task SaveAsync();
}
