using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IFactureRepository
{
    public Task<Facture> GetFactureByIdAsync(Guid id, Guid userCompanyId);

    public Task<IQueryable<Facture>> GetFacturesAsync(Guid userCompanyId);

    public Task AddAsync(Facture facture);

    public void Update(Facture facture);

    public void Delete(Facture facture);

    public Task<bool> ExistsByIdAsync(Guid id, Guid userCompanyId);

    public Task<bool> ExistsByNameAsync(string name, Guid userCompanyId);

    public void DeleteFactureDetailsByRange(IEnumerable<FactureDetail> factureDetails);

    public void DeletePaymentsByRange(IEnumerable<Payment> payments);
}
