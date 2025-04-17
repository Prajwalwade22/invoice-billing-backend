using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly LoanService _loanService;

        public LoanController(LoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet("calculate-emi")]
        public IActionResult CalculateEMI([FromQuery] LoanEmiDto loanDtos)
        {
            decimal emi = _loanService.CalculateEMI(loanDtos.LoanAmount, loanDtos.InterestRate, loanDtos.TenureMonths);
            return Ok(new { EMI = emi });
        }

        [HttpPost("apply")] 
        public async Task<IActionResult> ApplyForLoan([FromBody] LoanApplicationDto loanDto)
        {
            var loan = await _loanService.ApplyForLoanAsync(loanDto);
            return Ok(loan);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetLoanApplications(Guid userId)
        {
            var loans = await _loanService.GetLoanApplicationsByUserIdAsync(userId);
            return Ok(loans);
        }

        [HttpPut("update-status/{loanId}")]
        public async Task<IActionResult> UpdateLoanStatus(Guid loanId, [FromBody] string status)
        {
            var success = await _loanService.UpdateLoanStatusAsync(loanId, status);
            return success ? Ok(new { message = "Loan status updated" }) : BadRequest(new { message = "Failed to update status" });
        }
    }
}
