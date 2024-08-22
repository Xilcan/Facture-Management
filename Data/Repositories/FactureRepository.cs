using System.Data;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class FactureRepository : IFactureRepository
{
    private readonly FacturesManagementContext _context;

    public FactureRepository(FacturesManagementContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task AddAsync(Facture facture)
    {
        try
        {
            await _context.Factures.AddAsync(facture);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while adding Facture to database", ex);
        }
    }

    public void Delete(Facture facture)
    {
        try
        {
            _context.Remove(facture);
        }
        catch (DBConcurrencyException ex)
        {
            throw new DatabaseException("Number of zero rows affected while removing Facture from database", ex);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while removing Facture from database", ex);
        }
    }

    public void DeleteFactureDetailsByRange(IEnumerable<FactureDetail> factureDetails)
    {
        try
        {
            _context.FactureDetails.RemoveRange(factureDetails);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while removing facture details from database", ex);
        }
    }

    public void DeletePaymentsByRange(IEnumerable<Payment> payments)
    {
        try
        {
            _context.Payments.RemoveRange(payments);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while removing payments from database", ex);
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id, Guid userCompanyId)
    {
        try
        {
            return await _context.Factures.AnyAsync(f => f.Id == id && f.UserCompanyId == userCompanyId);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while checking if Facture exists by Id", ex);
        }
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid userCompanyId)
    {
        try
        {
            return await _context.Factures.AnyAsync(f => f.Name == name && f.UserCompanyId == userCompanyId);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while checking if Facture exists by Id", ex);
        }
    }

    public async Task<Facture> GetFactureByIdAsync(Guid id, Guid userCompanyId)
    {
        ArgumentNullException.ThrowIfNull(id);
        Facture? result;
        try
        {
            result = await _context.Factures
                .AsNoTracking()
            .Where(a => a.Id == id && a.UserCompanyId == userCompanyId)
            .Include(f => f.Company)
                .ThenInclude(c => c!.Address)
            .Include(f => f.UserCompany)
                .ThenInclude(c => c!.Address)
            .Include(f => f.FactureDetails)
                .ThenInclude(fd => fd.Products)
                    .ThenInclude(p => p.Category)
            .Include(f => f.PdfFiles)
            .Include(f => f.Payments)
            .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while getting Facture by Id from database", ex);
        }

        return result ?? throw new NotFoundException($"Cannot find the Facture with id = {id}");
    }

    public Task<IQueryable<Facture>> GetFacturesAsync(Guid userCompanyId)
    {
        try
        {
            var result = _context.Factures
                .Where(f => f.UserCompanyId == userCompanyId)
                .AsNoTracking()
            .Include(f => f.Company)
                .ThenInclude(c => c!.Address)
            .Include(f => f.UserCompany)
                .ThenInclude(c => c!.Address)
            .Include(f => f.FactureDetails)
                .ThenInclude(fd => fd.Products)
                    .ThenInclude(p => p.Category)
            .Include(f => f.PdfFiles)
            .Include(f => f.Payments);
            return Task.FromResult<IQueryable<Facture>>(result);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while getting all Facture from database", ex);
        }
    }

    public void Update(Facture facture)
    {
        try
        {
            _context.Factures.Update(facture);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while uppdating Facture in database", ex);
        }
    }
}
