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

    public async Task AddAsync(FacturePost facture, Guid userCompanyId)
    {
        try
        {
            var mappedFacture = _mapper.Map<Facture>(facture);
            if (await _unitOfWork.FactureRepository.ExistsByNameAsync(mappedFacture.Name, userCompanyId))
            {
                throw new BadRequestException($"Facture with name = {mappedFacture.Name} already exists.");
            }

            mappedFacture.UserCompanyId = userCompanyId;
            mappedFacture.UserCompany = await _unitOfWork.CompanyRepository.GetByIdAsync(userCompanyId, null);
            mappedFacture.Company = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.CompanyId, userCompanyId);

            var factureDetails = new List<FactureDetail>();
            foreach (var iteam in facture.FactureDetails)
            {
                var factureDetail = _mapper.Map<FactureDetail>(iteam);
                factureDetail.Products = await _unitOfWork.ProductRepository.GetByIdAsync(iteam.ProductId, userCompanyId);
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

    public async Task DeleteAsync(Guid factureId, Guid userCompanyId)
    {
        var facture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(factureId, userCompanyId);
        _unitOfWork.FactureRepository.Delete(facture);
        await _unitOfWork.SaveAsync();
    }

    public async Task<FullFactureGet> GetFactureByIdAsync(Guid id, Guid userCompanyId)
    {
        try
        {
            var facture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(id, userCompanyId);
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
        PagingDtoOption pagingDtoOption,
        Guid userCompanyId)
    {
        var factures = await _unitOfWork.FactureRepository.GetFacturesAsync(userCompanyId);
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

    public async Task UpdateAsync(FacturePut facture, Guid userCompanyId)
    {
        try
        {
            var existingFacture = await _unitOfWork.FactureRepository.GetFactureByIdAsync(facture.Id, userCompanyId);
            if (existingFacture.Name != facture.Name)
            {
                if (await _unitOfWork.FactureRepository.ExistsByNameAsync(facture.Name, userCompanyId))
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

            if (existingFacture.CompanyId != facture.CompanyId)
            {
                existingFacture.CompanyId = facture.CompanyId;
                existingFacture.Company = await _unitOfWork.CompanyRepository.GetByIdAsync(facture.CompanyId, userCompanyId);
            }

            _unitOfWork.FactureRepository.DeleteFactureDetailsByRange(existingFacture.FactureDetails);
            existingFacture.FactureDetails.Clear();
            foreach (var item in facture.FactureDetails)
            {
                var factureDetail = _mapper.Map<FactureDetail>(item);
                factureDetail.Products = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId, userCompanyId);
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
