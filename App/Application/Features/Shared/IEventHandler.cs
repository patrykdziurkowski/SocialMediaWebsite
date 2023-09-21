using System.Data;

namespace Application.Features.Shared
{
    public interface IEventHandler
    {
        Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction);
    }
}
