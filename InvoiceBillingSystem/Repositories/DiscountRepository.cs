using InvoiceBillingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class DiscountRepository:IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetDynamicDiscountAsync(Guid userId, decimal invoiceAmount)
        {
            var userInvoices = await _context.Invoices
                .Where(i => i.UserId == userId)
                .ToListAsync();


            int totalPaidInvoices = userInvoices.Count(i => i.Status == "Paid");

            decimal baseDiscount = 2.0m; //default

            if (totalPaidInvoices > 4)
                baseDiscount += 1.0m; 

            if (totalPaidInvoices > 10)
                baseDiscount += 2.0m; 

            decimal discountAmount = (baseDiscount / 100) * invoiceAmount;
            return Math.Round(discountAmount, 2);
        }
    }
}

