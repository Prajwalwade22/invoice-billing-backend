using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto chatMessage)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message);
            return Ok(new { message = "Message sent successfully!" });
        }
    }
}
