using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task LogActionAsync(string action, string performedBy, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                PerformedBy = performedBy,
                Details = details
            };

            await _auditLogRepository.LogActionAsync(log);
        }
    }

}
