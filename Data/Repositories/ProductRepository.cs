using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly FacturesManagementContext _context;

    public ProductRepository(FacturesManagementContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while adding product to database", ex);
        }
    }

    public void Delete(Product product)
    {
        try
        {
            _context.Products.Remove(product);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while removing product from database", ex);
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        try
        {
            return await _context.Products.AnyAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while checking if product exist by id", ex);
        }
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        try
        {
            return await _context.Products.AnyAsync(c => c.Name == name);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while checking if product exist by name", ex);
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving all product from database", ex);
        }
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        Product? result;
        try
        {
            result = await _context.Products.Where(c => c.Id == id)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving product by id from database", ex);
        }

        return result ?? throw new NotFoundException($"Cannot found product by id = {id}");
    }

    public void Update(Product product)
    {
        try
        {
            _context.Products.Update(product);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while updating product in database", ex);
        }
    }
}
