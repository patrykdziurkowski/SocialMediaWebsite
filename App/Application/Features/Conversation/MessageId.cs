using Application.Features.Shared;

namespace Application.Features.Chat
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
