using System.Collections.Generic;

namespace HandBook.Shared
{
    public interface IHasDomainEvents
    {
        IReadOnlyList<DomainEvent> UncommittedChanges();

        void MarkChangesAsCommitted();

        void Raise(DomainEvent @event);
    }
}