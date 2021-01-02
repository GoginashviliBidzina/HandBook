using HandBook.Shared;
using HandBook.Infrastructure.DataBase;

namespace HandBook.Infrastructure.EventDispatching
{
    public interface IHandleEvent<in TEvent>
    {
        void Handle(TEvent @event, DatabaseContext db);
    }
}
