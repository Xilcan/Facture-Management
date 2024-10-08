﻿using AutoMapper;
using Bussines.Dto.Company;
using Bussines.Services.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;

namespace Bussines.Services;

public class CompanyService : ICompanyService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(CompanyPost company, Guid userCompanyId)
    {
        try
        {
            var newCompany = _mapper.Map<Company>(company);
            if (await _unitOfWork.CompanyRepository.ExistsByNIPAsync(newCompany.NIP, userCompanyId))
            {
                throw new BadRequestException($"Company with NIP = {newCompany.NIP} already exists.");
            }

            newCompany.UserCompanyId = userCompanyId;

            await _unitOfWork.CompanyRepository.AddAsync(newCompany);
            await _unitOfWork.SaveAsync();
            return;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping CompanyPost to Company", ex);
        }
    }

    public async Task DeleteAsync(Guid id, Guid userCompanyId)
    {
        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id, userCompanyId);
        _unitOfWork.CompanyRepository.Delete(company);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<BriefCompanyGet>> GetAllAsync(Guid userCompanyId)
    {
        try
        {
            var companies = await _unitOfWork.CompanyRepository.GetAllAsync(userCompanyId);

            var mappedCompanies = new List<BriefCompanyGet>();
            foreach (var company in companies)
            {
                mappedCompanies.Add(_mapper.Map<BriefCompanyGet>(company));
            }

            return mappedCompanies;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping Company to BriefCompany", ex);
        }
    }

    public async Task<FullCompanyGet> GetByIdAsync(Guid id, Guid userCompanyId)
    {
        try
        {
            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id, userCompanyId);
            return _mapper.Map<FullCompanyGet>(company);
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping Company to FullCompanyGet", ex);
        }
    }

    public async Task UpdateAsync(FullCompanyPut company, Guid userCompanyId)
    {
        try
        {
            var existingCompany = await _unitOfWork.CompanyRepository.GetByIdAsync(company.Id, userCompanyId);
            existingCompany.Name = company.Name;
            if (existingCompany.NIP != company.NIP)
            {
                existingCompany.NIP = company.NIP;
                if (await _unitOfWork.CompanyRepository.ExistsByNIPAsync(existingCompany.NIP, userCompanyId))
                {
                    throw new BadRequestException($"Company with NIP = {existingCompany.NIP} already exists.");
                }
            }

            existingCompany.Address.HouseNumber = company.Address.HouseNumber;
            existingCompany.Address.City = company.Address.City;
            existingCompany.Address.LocalNumber = company.Address.LocalNumber;
            existingCompany.Address.Country = company.Address.Country;
            existingCompany.Address.PosteCode = company.Address.PosteCode;

            _unitOfWork.CompanyRepository.Update(existingCompany);
            await _unitOfWork.SaveAsync();
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occurred during mapping FullCompanyPut to Company", ex);
        }
    }
}
