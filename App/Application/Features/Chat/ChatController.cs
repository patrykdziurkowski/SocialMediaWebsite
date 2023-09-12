using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatRepository _chatRepository;

        public ChatController(ChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Claim chatterIdClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            int chatterId = int.Parse(chatterIdClaim.Value);

            Chat chat = await _chatRepository.GetAsync(chatterId);

            return Ok(chat);
        }
           
    }
}
