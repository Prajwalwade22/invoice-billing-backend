using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class InvoiceRepository:IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        private const decimal InterestRate = 0.02m; //2% divas

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(Guid invoiceId)
        {
            return await _context.Invoices.FindAsync(invoiceId);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(Guid userId)
        {
            return await _context.Invoices.Where(i => i.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateInvoiceStatusAsync(Guid invoiceId, string status)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync()
        {
            return await _context.Invoices
                .Where(i => i.Status == "Pending" && i.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<Invoice> GetInvoiceWithUserByIdAsync(Guid invoiceId)
        {
            return await _context.Invoices
                .Include(i => i.User) // ✅ Ensure User is Loaded
                .Select(i => new Invoice
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    Amount = i.Amount,
                    Status = i.Status,
                    DueDate = i.DueDate,
                    User = new User // ✅ Ensure Phone is included
                    {
                        Id = i.User.Id,
                        FullName = i.User.FullName,
                        Email = i.User.Email,
                        Phone = i.User.Phone // ✅ Ensure Phone is selected
                    }
                })
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }
        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            var existingInvoice = await _context.Invoices.FindAsync(invoice.Id);
            if (existingInvoice == null)
            {
                throw new Exception("Invoice not found.");
            }

            existingInvoice.PaidAmount = invoice.PaidAmount;
            existingInvoice.RemainingAmount = invoice.RemainingAmount;
            existingInvoice.IsFullyPaid = invoice.IsFullyPaid;
            existingInvoice.Status = invoice.Status;

            existingInvoice.DueDate = DateTime.SpecifyKind(invoice.DueDate, DateTimeKind.Utc);
            existingInvoice.CreatedAt = DateTime.SpecifyKind(invoice.CreatedAt, DateTimeKind.Utc);

            _context.Invoices.Update(existingInvoice);
            await _context.SaveChangesAsync();
        }


        public decimal CalculateOverdueInterest(Invoice invoice)
        {
            if (invoice.Status == "Paid") return 0;

            int overdueDays = (DateTime.UtcNow - invoice.DueDate).Days;
            if (overdueDays <= 0) return 0;

            decimal interestAmount = invoice.Amount * InterestRate * overdueDays;
            return Math.Round(interestAmount, 2);
        }

        public async Task ApplyOverdueInterestAsync(Guid invoiceId, decimal interestAmount)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice != null && invoice.Status != "Paid")
            {
                invoice.Amount += interestAmount;
                invoice.OverdueInterest += interestAmount; 
                await _context.SaveChangesAsync();
            }
        }
    }

}
