using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("Summary")]
        public async Task<IActionResult> GetReportSummary(string? startDate, string? endDate)
        {
            DateTime? start = null, end = null;

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStart))
            {
                start = DateTime.SpecifyKind(parsedStart, DateTimeKind.Utc);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEnd))
            {
                end = DateTime.SpecifyKind(parsedEnd, DateTimeKind.Utc);
            }

            var report = await _reportService.GetReportSummaryAsync(start, end);
            return Ok(report);
        }


        [HttpGet("DownloadPdf")]
        public async Task<IActionResult> DownloadReportPdf(string? startDate, string? endDate)
        {
            DateTime? start = null, end = null;

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStart))
            {
                start = DateTime.SpecifyKind(parsedStart, DateTimeKind.Utc);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEnd))
            {
                end = DateTime.SpecifyKind(parsedEnd, DateTimeKind.Utc);
            }

            var pdfBytes = await _reportService.GenerateReportPdfAsync(start, end);
            return File(pdfBytes, "application/pdf", "Report.pdf");
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadReportExcel(string? startDate, string? endDate)
        {
            DateTime? start = null, end = null;

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStart))
            {
                start = DateTime.SpecifyKind(parsedStart, DateTimeKind.Utc);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEnd))
            {
                end = DateTime.SpecifyKind(parsedEnd, DateTimeKind.Utc);
            }

            var excelBytes = await _reportService.GenerateReportExcelAsync(start, end);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }


    }
}
