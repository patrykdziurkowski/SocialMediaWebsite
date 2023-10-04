using Application.Features.Shared;

namespace Application.Features.Conversations
{
    public class ConversationId : DomainId
    {
        public ConversationId() : base(Guid.NewGuid())
        {
        }

        public ConversationId(Guid value) : base(value)
        {
        }

    }
}
