using InvoiceBillingSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogController(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetLogsByUser(string email)
        {
            var logs = await _auditLogRepository.GetLogsByUserAsync(email);
            return Ok(logs);
        }

        [HttpGet("AuditId/{id}")]
        public async Task<IActionResult> GetLogsByInvoice(Guid id)
        {
            var logs = await _auditLogRepository.GetLogsByInvoiceAsync(id);
            return Ok(logs);
        }
    }
}
