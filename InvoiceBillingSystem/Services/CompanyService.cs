using AutoMapper;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using Org.BouncyCastle.Crypto;

namespace InvoiceBillingSystem.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = companyDto.Name,
                Email = companyDto.Email,
                Phone = companyDto.Phone,
                Address = companyDto.Address
            };

            var createdCompany = await _companyRepository.CreateCompanyAsync(company);
            return _mapper.Map<CompanyDto>(createdCompany);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyId);
            return company == null ? null : _mapper.Map<CompanyDto>(company);
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }
    }
}
