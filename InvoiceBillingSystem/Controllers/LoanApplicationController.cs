using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly LoanApplicationService _loanApplicationService;

        public LoanApplicationController(LoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpPost("apply-loan")]
        public async Task<IActionResult> ApplyForLoan([FromBody] LoanApplicationDto loanDto)
        {
            await _loanApplicationService.ApplyForLoanAsync(loanDto);
            return Ok(new { message = "Loan application submitted successfully!" });
        }
    }
}
