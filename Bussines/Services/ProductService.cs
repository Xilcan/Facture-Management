using AutoMapper;
using Bussines.Dto.Product;
using Bussines.Services.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;

namespace Bussines.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(ProductPost product)
    {
        try
        {
            var newProduct = _mapper.Map<Product>(product);
            if (await _unitOfWork.ProductRepository.ExistsByNameAsync(newProduct.Name))
            {
                throw new BadRequestException($"Product with name = {newProduct.Name} already exists.");
            }

            var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(product.CategoryId);
            newProduct.Category = category;

            await _unitOfWork.ProductRepository.AddAsync(newProduct);
            await _unitOfWork.SaveAsync();
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping ProductPost to Product", ex);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<BriefProductGet>> GetAllAsync(Guid? categoryId)
    {
        try
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            var mappedProducts = new List<BriefProductGet>();
            foreach (var company in products)
            {
                mappedProducts.Add(_mapper.Map<BriefProductGet>(company));
            }

            return mappedProducts;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping Product to BriefProductGet", ex);
        }
    }

    public async Task<FullProductGet> GetByIdAsync(Guid id)
    {
        try
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return _mapper.Map<FullProductGet>(product);
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping Product to FullProductGet", ex);
        }
    }

    public async Task UpdateAsync(ProductPut product)
    {
        var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id);
        if (existingProduct.Name != product.Name)
        {
            existingProduct.Name = product.Name;
            if (await _unitOfWork.ProductRepository.ExistsByNameAsync(existingProduct.Name))
            {
                throw new BadRequestException($"Product with name = {product.Name} already exists.");
            }
        }

        existingProduct.Description = product.Description ?? string.Empty;
        existingProduct.Price = product.Price;
        existingProduct.TransactionType = product.TransactionType;
        var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(product.CategoryId);
        existingProduct.CategoryId = category.Id;
        existingProduct.Category = category;

        _unitOfWork.ProductRepository.Update(existingProduct);
        await _unitOfWork.SaveAsync();
    }
}
