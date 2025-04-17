using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentLinkRepository _paymentLinkRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IAuditLogService _auditLogService;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        public PaymentsController(IPaymentService paymentService,IInvoiceRepository invoiceRepository,IPaymentLinkRepository paymentLinkRepository,IPaymentRepository paymentRepository,IPaymentGatewayService paymentGatewayService,IAuditLogService auditLogService,INotificationService notificationService,IConfiguration configuration)
        {
            _paymentService = paymentService;
            _invoiceRepository = invoiceRepository;
            _paymentLinkRepository = paymentLinkRepository;
            _paymentRepository = paymentRepository;
            _paymentGatewayService = paymentGatewayService;
            _auditLogService = auditLogService;
            _notificationService = notificationService;
            _configuration = configuration;
        }

  

        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto paymentDto)
        {
            var paymentLink = await _paymentLinkRepository.GetByIdAsync(paymentDto.PaymentLinkId);
            if (paymentLink == null || paymentLink.IsUsed || paymentLink.ExpiryTime < DateTime.UtcNow)
            {
                await _auditLogService.LogActionAsync("Payment Attempt Failed", paymentDto.InvoiceId.ToString(), $"Invalid or expired payment link for {paymentDto.PaymentLinkId}");
                return BadRequest("Invalid or expired payment link.");
            }

            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(paymentLink.InvoiceId);
            if (invoice == null)
            {
                await _auditLogService.LogActionAsync("Payment Attempt Failed", paymentDto.InvoiceId.ToString(), $"Invoice not found for PaymentLink {paymentDto.PaymentLinkId}");
                return NotFound("Invoice not found.");
            }

            decimal remainingBalance = (decimal)(invoice.Amount - invoice.PaidAmount - invoice.DiscountApplied);
            if (paymentDto.Amount > remainingBalance)
            {
                return BadRequest($"Payment amount exceeds remaining balance. Remaining: {remainingBalance}");
            }

            string transactionId = await _paymentGatewayService.ProcessPaymentAsync(paymentDto.Gateway, paymentDto.Amount);
            if (string.IsNullOrEmpty(transactionId))
            {
                await _auditLogService.LogActionAsync("Payment Failed", paymentDto.InvoiceId.ToString(), $"Transaction failed for Invoice {invoice.Id}");
                return BadRequest("Payment failed.");
            }

            paymentLink.IsUsed = true;
            await _paymentLinkRepository.UpdatePaymentLinkAsync(paymentLink);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Amount = paymentDto.Amount,
                TransactionId = transactionId,
                IsSuccessful = true,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = paymentDto.Gateway
            };

            await _paymentRepository.ProcessPaymentAsync(payment);


            invoice.PaidAmount += paymentDto.Amount;
            invoice.RemainingAmount = (decimal)(invoice.Amount - invoice.PaidAmount - invoice.DiscountApplied);
            invoice.Status = invoice.RemainingAmount <= 0 ? "Paid" : "Partially Paid";
            invoice.IsFullyPaid = invoice.RemainingAmount <= 0 ? true : false;

            await _invoiceRepository.UpdateInvoiceAsync(invoice);

            await _auditLogService.LogActionAsync("Payment Successful", paymentDto.InvoiceId.ToString(),
                $"Invoice {invoice.Id} paid {paymentDto.Amount} using {paymentDto.Gateway}, Transaction ID: {transactionId}");

            await _notificationService.SendPaymentConfirmationAsync(invoice, payment);

            return Ok(new { Message = "Payment Successful!", TransactionId = transactionId, RemainingBalance = invoice.RemainingAmount });
        }





        [HttpGet("verify/{transactionId}")]
        public async Task<IActionResult> VerifyPayment(string transactionId)
        {
            var isVerified = await _paymentService.VerifyPaymentAsync(transactionId);
            return isVerified ? Ok(new { message = "Payment verified" }) : BadRequest(new { message = "Payment verification failed" });
        }

        [HttpGet("pay/{paymentLinkId}")]
        public async Task<IActionResult> ShowPaymentOptions(Guid paymentLinkId)
        {
            var paymentLink = await _paymentLinkRepository.GetByIdAsync(paymentLinkId);
            if (paymentLink == null || paymentLink.IsUsed || paymentLink.ExpiryTime < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired payment link.");
            }

            return Ok(new
            {
                InvoiceId = paymentLink.InvoiceId,
                PaymentGateways = new List<string> { "Stripe", "PayPal", "Razorpay" }
            });
        }


        [HttpGet("get-payment-link/{invoiceId}")]
        public async Task<IActionResult> GetOrGeneratePaymentLink(Guid invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            if (invoice.RemainingAmount <= 0)
            {
                return BadRequest("This invoice is already fully paid.");
            }

            var existingLink = await _paymentLinkRepository.GetActivePaymentLinkByInvoiceIdAsync(invoiceId);
            if (existingLink != null)
            {
                return Ok(new { PaymentLinkId = existingLink.Id, Url = existingLink.UniqueLink });
            }

            // Generate a new payment link
            var newPaymentLink = new PaymentLink
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                UniqueLink = $"{_configuration["ApiBaseUrl"]}/pay/{invoiceId}", 
                ExpiryTime = DateTime.UtcNow.AddHours(24), // Set expiry time
                IsUsed = false
            };

            await _paymentLinkRepository.SavePaymentLinkAsync(newPaymentLink);

            return Ok(new { PaymentLinkId = newPaymentLink.Id, Url = newPaymentLink.UniqueLink });
        }


    }
}
