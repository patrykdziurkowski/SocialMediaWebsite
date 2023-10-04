using Application.Features.Shared;

namespace Application.Features.Conversations
{
    public class MessageId : DomainId
    {
        public MessageId() : base(Guid.NewGuid())
        {
        }

        public MessageId(Guid value) : base(value)
        {
        }

    }
}
