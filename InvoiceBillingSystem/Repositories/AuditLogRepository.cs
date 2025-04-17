using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AuditLog>> GetLogsByUserAsync(string email)
        {
            return await _context.AuditLogs
                .Where(log => log.PerformedBy == email)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByInvoiceAsync(Guid id)
        {
            return await _context.AuditLogs
      .Where(log => log.Id == id)
      .OrderByDescending(log => log.Timestamp)
      .ToListAsync();
        }
    }

}
