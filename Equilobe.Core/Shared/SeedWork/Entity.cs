using MediatR;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Equilobe.Core.Shared.SeedWork;

public abstract class Entity
{
    Guid _Id;
    public virtual Guid Id
    {
        get
        {
            return _Id;
        }
        protected set
        {
            _Id = value;
        }
    }

    private List<INotification> _domainEvents;

    [NotMapped]
    [JsonIgnore]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity()
    {
        _Id = Guid.NewGuid();
        _domainEvents = [];
    }

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= [];
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
