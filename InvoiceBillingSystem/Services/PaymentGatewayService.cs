namespace InvoiceBillingSystem.Services
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        public async Task<string> ProcessPaymentAsync(string gateway, decimal amount)
        {
            switch (gateway.ToLower())
            {
                case "stripe":
                    return await ProcessStripePayment(amount);
                case "paypal":
                    return await ProcessPayPalPayment(amount);
                case "razorpay":
                    return await ProcessRazorpayPayment(amount);
                default:
                    throw new Exception("Unsupported payment gateway.");
            }
        }

        private async Task<string> ProcessStripePayment(decimal amount)
        {
            await Task.Delay(500);
            return $"STRIPE_TXN_{Guid.NewGuid()}";
        }

        private async Task<string> ProcessPayPalPayment(decimal amount)
        {
            await Task.Delay(500);
            return $"PAYPAL_TXN_{Guid.NewGuid()}";
        }

        private async Task<string> ProcessRazorpayPayment(decimal amount)
        {
            await Task.Delay(500);
            return $"RAZORPAY_TXN_{Guid.NewGuid()}";
        }
    }
}