using Application.Features.Shared;

namespace Application.Features.Chat.Interfaces
{
    public interface IConversationEventHandlerFactory
    {
        IEventHandler GetHandler(DomainEvent domainEvent);
    }
}