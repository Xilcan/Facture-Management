using AutoMapper;
using Bussines.Dto.Factures;
using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Filters.FactureFilters.Models;
using Bussines.Services.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using Interface.ExceptionsHandling.Exceptions;
using Interface.FiltersInterface.PagingFilterInterface;
using Microsoft.EntityFrameworkCore;

namespace Bussines.Services;

public class FactureService : IFactureService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly IPagingOptionExtension _pagingExtension;
    private readonly IMapper _mapper;
    private readonly IFactureFilterPipeline _filterPipeline;

    public FactureService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPdfService pdfService,
        IPagingOptionExtension pagingExtension,
        IFactureFilterPipeline filterPipeline)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(pdfService);
        ArgumentNullException.ThrowIfNull(pagingExtension);
        ArgumentNullException.ThrowIfNull(filterPipeline);

        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _pdfService = pdfService;
        _pagingExtension = pagingExtension;
        _filterPipeline = filterPipeline;
    }

    public async Task AddAsync(FacturePost facture)
    {
        try
        {
            var mappedFacture = _mapper.Map<Facture>(facture);
            if (await _unitOfWork.FactureRepository.ExistsByNameAsync(mappedFacture.Name))
            {
                throw new BadRequestException($"Facture with name = {mappedFacture.Name} already exists.");
            }

            mappedFacture.UserCompany = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.UserCompanyId);
            mappedFacture.Company = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.CompanyId);

            var factureDetails = new List<FactureDetail>();
            foreach (var iteam in facture.FactureDetails)
            {
                var factureDetail = _mapper.Map<FactureDetail>(iteam);
                factureDetail.Products = await _unitOfWork.ProductRepository.GetByIdAsync(iteam.ProductId);
                factureDetails.Add(factureDetail);
            }

            mappedFacture.FactureDetails = factureDetails;

            var pdf = _pdfService.CreatePdfFile(mappedFacture);
            mappedFacture.PdfFiles =
            [
                new()
                {
                    Data = pdf,
                    Name = facture.Name + "_" + 1,
                    FactureId = mappedFacture.Id,
                    Facture = mappedFacture,
                },
            ];

            await _unitOfWork.FactureRepository.AddAsync(mappedFacture);
            await _unitOfWork.SaveAsync();
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occured during mapping FacturePost to Facture", ex);
        }
    }

    public async Task DeleteAsync(Guid factureId)
    {
        var facture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(factureId);
        _unitOfWork.FactureRepository.Delete(facture);
        await _unitOfWork.SaveAsync();
    }

    public async Task<FullFactureGet> GetFactureByIdAsync(Guid id)
    {
        try
        {
            var facture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(id);
            var mappedFacture = _mapper.Map<FullFactureGet>(facture);
            return mappedFacture;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occured during mapping Facture to FullFactureGet", ex);
        }
    }

    public async Task<IEnumerable<BriefFacturesGet>> GetFacturesAsync(
        FactureFilterCriteria factureSearchModel,
        FactureSortOptions factureSortOptions,
        PagingDtoOption pagingDtoOption)
    {
        var factures = await _unitOfWork.FactureRepository.GetFacturesAsync();
        var pagingOptions = _pagingExtension.GetPagingOption(pagingDtoOption);
        var factureFilterContext = new FactureFilterContext()
        {
            Query = factures,
            Criteria = factureSearchModel,
            PagingOption = pagingOptions,
            SortingOption = factureSortOptions,
        };

        _filterPipeline.Execute(factureFilterContext);

        try
        {
            var mappedFactures = _mapper.Map<IEnumerable<BriefFacturesGet>>(await factureFilterContext.Query.ToListAsync());
            return mappedFactures;
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occured during mapping Facture to FullFactureGet", ex);
        }
    }

    public async Task UpdateAsync(FacturePut facture)
    {
        try
        {
            var existingFacture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(facture.Id);
            if (existingFacture.Name != facture.Name)
            {
                if (await _unitOfWork.FactureRepository.ExistsByNameAsync(facture.Name))
                {
                    throw new BadRequestException($"Facture with name = {facture.Name} already exists.");
                }

                existingFacture.Name = facture.Name;
            }

            existingFacture.NumberFactures = facture.NumberFactures;
            existingFacture.Comment = facture.Comment;
            existingFacture.PaymentDate = facture.PaymentDate;
            existingFacture.SaleDate = facture.SaleDate;
            existingFacture.City = facture.City;

            if (existingFacture.UserCompanyId != facture.UserCompanyId)
            {
                existingFacture.UserCompanyId = facture.UserCompanyId;
                existingFacture.UserCompany = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.UserCompanyId);
            }

            if (existingFacture.CompanyId != facture.CompanyId)
            {
                existingFacture.CompanyId = facture.CompanyId;
                existingFacture.Company = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.CompanyId);
            }

            _unitOfWork.FactureRepository.DeleteFactureDetailsByRange(existingFacture.FactureDetails);
            existingFacture.FactureDetails.Clear();
            foreach (var item in facture.FactureDetails)
            {
                var factureDetail = _mapper.Map<FactureDetail>(item);
                factureDetail.Products = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                existingFacture.FactureDetails.Add(factureDetail);
            }

            _unitOfWork.FactureRepository.DeletePaymentsByRange(existingFacture.Payments);
            existingFacture.Payments.Clear();
            foreach (var item in facture.Payments)
            {
                var peyment = _mapper.Map<Payment>(item);
                existingFacture.Payments.Add(peyment);
            }

            var pdf = _pdfService.CreatePdfFile(existingFacture);
            existingFacture.PdfFiles.Add(new PdfFile()
            {
                Data = pdf,
                Name = facture.Name + "_" + (existingFacture.PdfFiles.Count + 1),
                FactureId = existingFacture.Id,
                Facture = existingFacture,
            });

            _unitOfWork.FactureRepository.Update(existingFacture);
            await _unitOfWork.SaveAsync();
        }
        catch (AutoMapperMappingException ex)
        {
            throw new ServiceException("An error occured during mapping Facture to FullFactureGet", ex);
        }
    }
}
