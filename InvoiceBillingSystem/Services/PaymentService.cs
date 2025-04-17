using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using Microsoft.AspNet.Identity;

namespace InvoiceBillingSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly INotificationService _NotificationService;
        private readonly IAuditLogService _auditLogService;
        private readonly IOtpService _otpService;
        public PaymentService(IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository,INotificationService notificationService,IAuditLogService auditLogService,IOtpService otpService)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _NotificationService = notificationService;
            _auditLogService = auditLogService;
            _otpService = otpService;
        }

        //public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentDto)
        //{
        //    var invoice = await _invoiceRepository.GetInvoiceWithUserByIdAsync(paymentDto.InvoiceId);
        //    if (invoice == null || invoice.User == null)
        //    {
        //        throw new Exception("Invoice or associated User not found.");
        //    }

        //    //decimal amountToPay = invoice.Amount;
        //    decimal amountToPay= invoice.Amount - (invoice.DiscountApplied ?? 0);

        //    var payment = new Payment
        //    {
        //        Id = Guid.NewGuid(),
        //        InvoiceId = invoice.Id,
        //        Amount = amountToPay, 
        //        TransactionId = Guid.NewGuid().ToString(),
        //        IsSuccessful = true,
        //        PaymentDate = DateTime.UtcNow,
        //        PaymentMethod=paymentDto.PaymentMethod
        //    };

        //    await _paymentRepository.ProcessPaymentAsync(payment);

        //    await _invoiceRepository.UpdateInvoiceStatusAsync(invoice.Id, "Paid");

        //    await _NotificationService.SendPaymentConfirmationAsync(invoice, payment);

        //    await _auditLogService.LogActionAsync("Payment Processed", invoice.User.Email,
        //        $"Paid ${amountToPay} for Invoice {invoice.Id}");

        //    return new PaymentResponseDto
        //    {
        //        TransactionId = payment.TransactionId,
        //        IsSuccessful =true,
        //        AmountPaid = amountToPay 
        //    };
        //}


        public async Task<bool> VerifyPaymentAsync(string transactionId)
        {
            var payment = await _paymentRepository.GetPaymentByTransactionIdAsync(transactionId);
            return payment != null && payment.IsSuccessful;
        }


        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentDto)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(paymentDto.InvoiceId);
            if (invoice == null)
                throw new Exception("Invoice not found.");

            if (invoice.IsFullyPaid)
                throw new Exception("This invoice is already fully paid.");

            if (paymentDto.Amount > invoice.RemainingAmount)
                throw new Exception($"Payment amount exceeds remaining balance. Remaining: {invoice.RemainingAmount}");

            // Record Payment
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                Amount = paymentDto.Amount,
                TransactionId = Guid.NewGuid().ToString(),
                IsSuccessful = true,
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.ProcessPaymentAsync(payment);

            invoice.PaidAmount += paymentDto.Amount;
            invoice.RemainingAmount -= paymentDto.Amount;

            if (invoice.RemainingAmount == 0)
            {
                invoice.IsFullyPaid = true;
                invoice.Status = "Paid";
            }

            await _invoiceRepository.UpdateInvoiceAsync(invoice);

            await _NotificationService.SendPaymentConfirmationAsync(invoice, payment);

            return new PaymentResponseDto
            {
                TransactionId = payment.TransactionId,
                IsSuccessful = true,
                AmountPaid = paymentDto.Amount,
                //RemainingAmount = invoice.RemainingAmount
            };
        }




    }
}