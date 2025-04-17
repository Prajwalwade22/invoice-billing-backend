using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly InvestmentService _investmentService;

        public InvestmentController(InvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferMoney([FromBody] InvestmentTransferDto transferDto)
        {
            await _investmentService.TransferMoneyAsync(transferDto);
            return Ok(new { message = "Transfer request submitted successfully!" });
        }
    }
}
