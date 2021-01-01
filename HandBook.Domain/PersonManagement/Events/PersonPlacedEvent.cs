using HandBook.Shared;

namespace HandBook.Domain.PersonManagement.Events
{
    public class PersonPlacedEvent : DomainEvent
    {
        public Person Person { get; private set; }

        public PersonPlacedEvent(Person person)
        {
            Person = person;
        }
    }
}
