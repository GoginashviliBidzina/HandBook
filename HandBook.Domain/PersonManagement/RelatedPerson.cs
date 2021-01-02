using HandBook.Shared;
using Newtonsoft.Json;

namespace HandBook.Domain.PersonManagement
{
    public class RelatedPerson : Entity
    {
        public int PersonId { get; private set; }

        [JsonIgnore]
        public virtual Person Person { get; private set; }

        public int RelatedPersonId { get; private set; }

        public RelationshipType RelationshipType { get; private set; }

        public  RelatedPerson()
        {

        }

        [JsonConstructor]
        public RelatedPerson(int personId,
                             int relatedPersonId,
                             RelationshipType relationshipType)
        {
            PersonId = personId;
            RelatedPersonId = relatedPersonId;
            RelationshipType = relationshipType;
        }
    }
}
