namespace Application.Features.Shared
{
    public abstract class AggreggateRoot
    {
        private readonly List<DomainEvent> _domainEvents;

        public AggreggateRoot()
        {
            _domainEvents = new List<DomainEvent>();
        }

        public IEnumerable<DomainEvent> DomainEvents => _domainEvents;

        public void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

    }
}
