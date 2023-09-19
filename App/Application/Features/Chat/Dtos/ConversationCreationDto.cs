using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat.Dtos
{
    public class ConversationCreationDto
    {
        public List<Guid>? ConversationMemberIds { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
