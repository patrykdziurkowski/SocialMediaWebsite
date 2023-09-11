using Application.Features.Shared;

namespace Application.Features.Chat.Interfaces
{
    public interface IChatEventHandlerFactory
    {
        IEventHandler GetHandler(DomainEvent domainEvent);
    }
}