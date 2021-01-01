using HandBook.Domain.PersonManagement;

namespace HandBook.Application.Commands.Person.Models
{
    public class RelatedPersonDto
    {
        public int RelatedPersonId { get; set; }

        public RelationshipType RelationshipType { get; set; }
    }
}
