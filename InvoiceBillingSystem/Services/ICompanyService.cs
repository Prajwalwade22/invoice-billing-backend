using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
        Task<CompanyDto?> GetCompanyByIdAsync(Guid companyId);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
    }
}
