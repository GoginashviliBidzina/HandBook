using HandBook.Shared;

namespace HandBook.Domain.PersonManagement.Events
{
    public class PersonUpdatedEvent : DomainEvent
    {
        public Person Person { get; private set; }

        public PersonUpdatedEvent(Person person)
        {
            Person = person;
        }
    }
}
