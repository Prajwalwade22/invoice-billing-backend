namespace InvoiceBillingSystem.Services
{
    public interface IPaymentGatewayService
    {
        Task<string> ProcessPaymentAsync(string gateway, decimal amount);
    }
}
