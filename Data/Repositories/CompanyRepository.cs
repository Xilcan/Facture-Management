using System.Data;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly FacturesManagementContext _context;

    public CompanyRepository(FacturesManagementContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task AddAsync(Company company)
    {
        try
        {
            await _context.Companies.AddAsync(company);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while adding Company to database", ex);
        }
    }

    public void Delete(Company company)
    {
        try
        {
            _context.Remove(company);
        }
        catch (DBConcurrencyException ex)
        {
            throw new DatabaseException("Number of zero rows affected while removing Company from database", ex);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while removing Company from database", ex);
        }
    }

    public void DeleteCompanyAdressesByRange(IEnumerable<CompanyAddress> companyAddresses)
    {
        try
        {
            _context.CompanyAddresses.RemoveRange(companyAddresses);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An error occured while deleting Company Adresses by Range", ex);
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        try
        {
            return await _context.Companies.AnyAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while checking if Company exist by id", ex);
        }
    }

    public async Task<bool> ExistsByNIPAsync(long nip)
    {
        try
        {
            return await _context.Companies.AnyAsync(c => c.NIP == nip);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while checking if Company exist by id", ex);
        }
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        try
        {
            return await _context.Companies
                .Include(c => c.Address)
                .Include(c => c.Factures)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving all Company from database", ex);
        }
    }

    public async Task<Company> GetByIdAsync(Guid id)
    {
        Company result;
        try
        {
            result = await _context.Companies.Where(c => c.Id == id)
                .Include(c => c.Address)
                .Include(c => c.Factures)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving Company by id from database", ex);
        }

        return result ?? throw new NotFoundException($"Cannot found Company by id = {id}");
    }

    public void Update(Company company)
    {
        try
        {
            _context.Companies.Update(company);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while updating Company in database", ex);
        }
    }
}
