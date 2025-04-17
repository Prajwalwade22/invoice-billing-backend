using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto invoiceDto);
        Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId);
        Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId);
        Task<bool> UpdateInvoiceStatusAsync(Guid invoiceId, string status);
    }
}
