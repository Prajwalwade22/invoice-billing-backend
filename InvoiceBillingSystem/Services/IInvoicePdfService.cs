namespace InvoiceBillingSystem.Services
{
    public interface IInvoicePdfService
    {
        byte[] GenerateInvoicePdf(Guid invoiceId);
    }
}
