using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentDto);
        Task<bool> VerifyPaymentAsync(string transactionId);
    }
}
