using Application.Features.Shared;

namespace Application.Features.Chat
{
    public interface IChatEventHandlerFactory
    {
        IEventHandler GetHandler(DomainEvent domainEvent);
    }
}