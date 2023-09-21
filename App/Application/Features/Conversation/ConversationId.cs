using Application.Features.Shared;

namespace Application.Features.Chat
{
    public class ConversationId : DomainId
    {
        public ConversationId() : base(Guid.NewGuid())
        {
        }

        public ConversationId(Guid value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
