using InvoiceBillingSystem.Attributes;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoicePdfService _invoicePdfService;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IRefundService _refundService;
    
       
        public InvoicesController(IInvoiceService invoiceService,IInvoicePdfService invoicePdfService,IRefundService refundService)
        {
            _invoiceService = invoiceService;
            _invoicePdfService = invoicePdfService;
            _googleDriveService = new GoogleDriveService();
            _refundService = refundService; 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        //  [RequireRole("Admin", "Accountant")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto invoiceDto)
        {
            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(invoiceDto);
                return Ok(invoice);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{invoiceId}")]
      // [RequireRole("Admin", "Accountant", "Customer")]

        public async Task<IActionResult> GetInvoiceById(Guid invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            return invoice != null ? Ok(invoice) : NotFound(new { message = "Invoice not found" });
        }

        [HttpGet("user/{userId}")]
        //[RequireRole("Admin", "Accountant", "Customer")]
        public async Task<IActionResult> GetInvoicesByUserId(Guid userId)
        {
            var invoices = await _invoiceService.GetInvoicesByUserIdAsync(userId);
            return Ok(invoices);
        }

        [HttpPut("update-status/{invoiceId}")]
        public async Task<IActionResult> UpdateInvoiceStatus(Guid invoiceId, [FromBody] string status)
        {
            var success = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, status);
            return success ? Ok(new { message = "Invoice status updated" }) : BadRequest(new { message = "Failed to update status" });
        }

        //[HttpGet("GeneratePDF/{invoiceId}")]
        //public IActionResult GenerateInvoicePdf(Guid invoiceId)
        //{
        //    try
        //    {
        //        var pdfBytes = _invoicePdfService.GenerateInvoicePdf(invoiceId);
        //        return File(pdfBytes, "application/pdf", "InvoiceBillingSystem.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("download/{invoiceId}")]
        public IActionResult DownloadInvoice(Guid invoiceId)
        {
            var pdfBytes = _invoicePdfService.GenerateInvoicePdf(invoiceId);
            if (pdfBytes == null)
            {
                return NotFound("Invoice not found.");
            }

            return File(pdfBytes, "application/pdf", $"Invoice_{invoiceId}.pdf");
        }


        [HttpGet("test-upload")]
        public async Task<IActionResult> TestUpload()
        {
            byte[] testFile = System.IO.File.ReadAllBytes("wwwroot/invoices/Prajwal.P.Wade_MCA.pdf"); 
            string folderId = "1qo5YmNiOfKEePHUASY1TG4PY8xGKuWvp";  // folder ID of drive

            try
            {
                string fileUrl = await _googleDriveService.UploadFileAsync(testFile, "TestInvoice.pdf", folderId);
                return Ok(new { message = "File uploaded successfully!", link = fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("refund")]
        public async Task<IActionResult> IssueRefund([FromBody] RefundRequestDto refundDto)
        {
            try
            {
                await _refundService.IssueRefundAsync(refundDto.InvoiceId, refundDto.RefundAmount, refundDto.Reason);
                return Ok(new { Message = "Refund issued successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



       
    }
}
