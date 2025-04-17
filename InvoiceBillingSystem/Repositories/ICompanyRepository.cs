using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(Guid companyId);
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
    }
}
