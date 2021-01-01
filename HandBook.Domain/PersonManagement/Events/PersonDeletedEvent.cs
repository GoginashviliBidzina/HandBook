using HandBook.Shared;

namespace HandBook.Domain.PersonManagement.Events
{
    public class PersonDeletedEvent : DomainEvent
    {
        public int Id { get; private set; }

        public PersonDeletedEvent(int id)
        {
            Id = id;
        }
    }
}
