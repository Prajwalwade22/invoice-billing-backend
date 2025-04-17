using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface IReportService
    {
        Task<ReportSummaryDto> GetReportSummaryAsync(DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateReportPdfAsync(DateTime? startDate, DateTime? endDate);
        Task<byte[]> GenerateReportExcelAsync(DateTime? startDate, DateTime? endDate);
    }
}
