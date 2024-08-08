using AutoMapper;
using Bussines.Dto;
using Bussines.Dto.Adress;
using Bussines.Dto.Company;
using Bussines.Dto.FactureDetails;
using Bussines.Dto.Factures;
using Bussines.Dto.Payment;
using Bussines.Dto.Product;
using Bussines.Dto.ProductCategory;
using Data.Models;

namespace Bussines.AutoMapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CompanyPost, Company>();
        CreateMap<AddressPost, CompanyAddress>();
        CreateMap<Company, BriefCompanyGet>();
        CreateMap<Company, FullCompanyGet>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        CreateMap<FullCompanyPut, Company>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        CreateMap<CompanyAddress, AddressGet>();

        CreateMap<ProductCategoryPost, ProductCategory>();
        CreateMap<ProductCategory, BriefProductCategoryGet>();
        CreateMap<ProductCategory, FullProductCategoryGet>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<Product, BriefProductGet>();
        CreateMap<Product, FullProductGet>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        CreateMap<ProductPost, Product>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? "Brak uwag"));
        CreateMap<ProductPut, Product>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<FacturePost, Facture>()
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
            .ForMember(dest => dest.FactureDetails, opt => opt.MapFrom(src => src.FactureDetails));
        CreateMap<FactureDetailsPost, FactureDetail>()
            .ForMember(dest => dest.UnitPriceNetto, opt => opt.MapFrom(src => src.UnitPriceBrutto / (1 + (src.Vat / 100))));
        CreateMap<PaymentPost, Payment>();
        CreateMap<Facture, FullFactureGet>()
            .ForMember(dest => dest.UserCompany, opt => opt.MapFrom(src => src.UserCompany))
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.FactureDetails, opt => opt.MapFrom(src => src.FactureDetails))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
            .ForMember(dest => dest.PdfFiles, opt => opt.MapFrom(src => src.PdfFiles));

        CreateMap<FactureDetail, FactureDetailsGet>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Products));
        CreateMap<Payment, PaymentGetPut>().ReverseMap();

        CreateMap<Facture, BriefFacturesGet>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserCompany.Name))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.FactureDetails.Sum(a => a.UnitPriceBrutto * a.Quantity)));
        CreateMap<FactureDetailsPut, FactureDetail>()
            .ForMember(dest => dest.UnitPriceNetto, opt => opt.MapFrom(src => src.UnitPriceBrutto / (1 + (src.Vat / 100))));

        CreateMap<PdfFile, PdfFileGet>();
    }
}
