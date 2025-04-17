using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Invoices.Where(i => i.Status == "Paid");

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(i => i.CreatedAt >= startDate.Value.ToUniversalTime() &&
                                         i.CreatedAt <= endDate.Value.ToUniversalTime());
            }

            return await query.SumAsync(i => i.Amount);
        }


        public async Task<int> GetPendingInvoiceCountAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Invoices.Where(i => i.Status == "Pending");

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate);
            }

            return await query.CountAsync();
        }

        public async Task<decimal> GetTotalTaxCollectedAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Invoices.Where(i => i.Status == "Paid");

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate);
            }

            return await query.SumAsync(i => i.Amount * 0.18m);
        }

        public async Task<List<TopCustomerDto>> GetTopCustomersAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Invoices.Where(i => i.Status == "Paid");

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate);
            }

            return await query
                .GroupBy(i => i.UserId)
                .Select(g => new TopCustomerDto
                {
                    UserId = g.Key,
                    TotalSpent = g.Sum(i => i.Amount)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();
        }
    }
}

