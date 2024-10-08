﻿using AutoMapper;
using Bussines.Dto.ProductCategory;
using Bussines.Services.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;

namespace Bussines.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(ProductCategoryPost productCategory, Guid userCompanyId)
    {
        try
        {
            if (await _unitOfWork.ProductCategoryRepository.ExistByNameAsync(productCategory.Name, userCompanyId))
            {
                throw new BadRequestException($"ProductCategory with name = {productCategory.Name} already exists");
            }

            var mappedProductCategory = _mapper.Map<ProductCategory>(productCategory);

            mappedProductCategory.UserCompanyId = userCompanyId;

            await _unitOfWork.ProductCategoryRepository.AddAsync(mappedProductCategory);
            await _unitOfWork.SaveAsync();
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping ProductCategoryPost to ProductCategory", ex);
        }
    }

    public async Task DeleteAsync(Guid id, Guid userCompanyId)
    {
        var productCategory = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id, userCompanyId);
        _unitOfWork.ProductCategoryRepository.Delete(productCategory);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<BriefProductCategoryGet>> GetAllAsync(Guid userCompanyId)
    {
        try
        {
            var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsync(userCompanyId);

            var mappedProductCategory = new List<BriefProductCategoryGet>();
            foreach (var productCategory in productCategories)
            {
                mappedProductCategory.Add(_mapper.Map<BriefProductCategoryGet>(productCategory));
            }

            return mappedProductCategory;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping ProductCategory to BrieProductCategory", ex);
        }
    }

    public async Task<FullProductCategoryGet> GetByIdAsync(Guid id, Guid userCompanyId)
    {
        try
        {
            var productCategory = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id, userCompanyId);
            return _mapper.Map<FullProductCategoryGet>(productCategory);
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping ProductCategory to FullProductCategoryGet", ex);
        }
    }

    public async Task UpdateAsync(BriefProductCategoryGet productCategory, Guid userCompanyId)
    {
        var existingProductCategory = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(productCategory.Id, userCompanyId);

        if (existingProductCategory.Name != productCategory.Name)
        {
            existingProductCategory.Name = productCategory.Name;
            if (await _unitOfWork.ProductCategoryRepository.ExistByNameAsync(productCategory.Name, userCompanyId))
            {
                throw new BadRequestException($"ProductCategory with name = {productCategory.Name} already exists");
            }
        }

        existingProductCategory.Description = productCategory.Description;

        _unitOfWork.ProductCategoryRepository.Update(existingProductCategory);
        await _unitOfWork.SaveAsync();
    }
}
