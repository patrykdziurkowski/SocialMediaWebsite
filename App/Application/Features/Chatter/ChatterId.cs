using Application.Features.Shared;

namespace Application.Features.Chatter
{
    public class ChatterId : DomainId
    {
        public ChatterId() : base(Guid.NewGuid())
        {
        }

        public ChatterId(Guid value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
