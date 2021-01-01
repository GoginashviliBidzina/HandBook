using HandBook.Shared;

namespace HandBook.Domain.PersonManagement.Events
{
    public class RelatedPersonAddedEvent : DomainEvent
    {
        public Person Person { get; private set; }

        public RelatedPersonAddedEvent(Person person)
        {
            Person = person;
        }
    }
}
