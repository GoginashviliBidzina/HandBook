using System.Collections.Generic;
using HandBook.Infrastructure.DataBase;

namespace HandBook.Infrastructure.EventDispatching
{
    public interface IInternalEventDispatcher<TDomainEvent>
    {
        void Dispatch(IReadOnlyList<TDomainEvent> domainEvents, DatabaseContext dbContext);
    }
}
