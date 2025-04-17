using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class PaymentLinkRepository : IPaymentLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentLinkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentLink?> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.PaymentLinks.FirstOrDefaultAsync(pl => pl.InvoiceId == invoiceId && !pl.IsUsed);
        }

        public async Task<PaymentLink?> GetByIdAsync(Guid paymentLinkId)
        {
            return await _context.PaymentLinks.FirstOrDefaultAsync(pl => pl.Id == paymentLinkId);
        }

        public async Task SavePaymentLinkAsync(PaymentLink paymentLink)
        {
            _context.PaymentLinks.Add(paymentLink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentLinkAsync(PaymentLink paymentLink)
        {
            _context.PaymentLinks.Update(paymentLink);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentLink> GetActivePaymentLinkByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.PaymentLinks
                .Where(pl => pl.InvoiceId == invoiceId && !pl.IsUsed && pl.ExpiryTime > DateTime.UtcNow)
                .OrderByDescending(pl => pl.CreatedAt) 
                .FirstOrDefaultAsync();
        }
    }
}
