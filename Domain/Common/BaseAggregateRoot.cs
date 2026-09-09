namespace Domain.Common
{
    public abstract class BaseAggregateRoot : BaseEntity , IAggregateRoot
    {

        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get { return _domainEvents.AsReadOnly(); } }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow; 
        public DateTimeOffset UpdatedAt { get; private set; }

    }
}
