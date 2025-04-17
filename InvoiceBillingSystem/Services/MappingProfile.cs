using AutoMapper;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
        }
    }
}
