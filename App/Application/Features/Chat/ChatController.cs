using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class ChatController : Controller
    {
        private readonly ChatRepository _chatRepository;

        public ChatController(ChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<IActionResult> Index()
        {

            Chat chat = await _chatRepository.GetAsync(1);

            return Ok(chat.ChatterId);
        }
           
    }
}
