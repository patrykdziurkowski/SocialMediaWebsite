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
        public IActionResult Index()
        {
            return Ok("Test");
        }
           
    }
}
