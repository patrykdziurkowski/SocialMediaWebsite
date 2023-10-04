using Application.Features.Shared;

namespace Application.Features.Authentication
{
    public class UserId : DomainId
    {
        public UserId() : base(Guid.NewGuid())
        {
        }

        public UserId(Guid value) : base(value)
        {
        }
    }
}
