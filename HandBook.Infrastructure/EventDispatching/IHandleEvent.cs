using HandBook.Shared;
using HandBook.Infrastructure.DataBase;

namespace HandBook.Infrastructure.EventDispatching
{
    public interface IHandleEvent<in TEvent> where TEvent : DomainEvent
    {
        void Handle(TEvent @event, DatabaseContext db);
    }
}
