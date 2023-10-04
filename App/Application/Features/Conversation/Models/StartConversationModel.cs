﻿namespace Application.Features.Conversations.Dtos
{
    public class StartConversationModel
    {
        public List<Guid>? ConversationMemberIds { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
