using Application.Features.Shared;

namespace Application.Features.Conversations.Interfaces
{
    public interface IConversationEventHandlerFactory
    {
        IEventHandler GetHandler(DomainEvent domainEvent);
    }
}