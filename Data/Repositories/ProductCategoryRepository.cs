using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly FacturesManagementContext _context;

    public ProductCategoryRepository(FacturesManagementContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public async Task AddAsync(ProductCategory productCategory)
    {
        try
        {
            await _context.ProductCategories.AddAsync(productCategory);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while adding product category to database", ex);
        }
    }

    public void Delete(ProductCategory productCategory)
    {
        try
        {
            _context.ProductCategories.Remove(productCategory);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while removing product category from database", ex);
        }
    }

    public async Task<bool> ExistByNameAsync(string name)
    {
        try
        {
            return await _context.ProductCategories.AnyAsync(c => c.Name == name);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while checking if ProductCategory exist by name", ex);
        }
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        try
        {
            return await _context.ProductCategories
                .Include(pc => pc.Products)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving all product categories from database", ex);
        }
    }

    public async Task<ProductCategory> GetByIdAsync(Guid id)
    {
        ProductCategory? result;
        try
        {
            result = await _context.ProductCategories.Where(c => c.Id == id)
                .Include(pc => pc.Products)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while retriving product category by id from database", ex);
        }

        return result ?? throw new NotFoundException($"Cannot found product category by id = {id}");
    }

    public void Update(ProductCategory productCategory)
    {
        try
        {
            _context.ProductCategories.Update(productCategory);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("An unnexpected error occured while updating product category in database", ex);
        }
    }
}
