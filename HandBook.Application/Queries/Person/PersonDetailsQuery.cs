using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HandBook.Domain.PersonManagement;
using HandBook.Application.Infrastructure;
using HandBook.Domain.PersonManagement.ReadModels;

namespace HandBook.Application.Queries.Person
{
    public class PersonDetailsQuery : Query<PersonDetailsQueryResult>
    {
        public int Id { get; set; }

        public async override Task<QueryExecutionResult<PersonDetailsQueryResult>> ExecuteAsync()
        {
            try
            {
                var person = await _db.Set<PersonReadModel>()
                                      .FirstOrDefaultAsync(personReadModel => personReadModel.AggregateRootId == Id);

                if (person == null)
                    return await FailAsync(ErrorCode.NotFound);

                var relatedPersons = JsonConvert.DeserializeObject<IEnumerable<RelatedPerson>>(person.RelatedPersonJson);

                var result = new PersonDetailsQueryResult(person.AggregateRootId,
                                                          person.FirstName,
                                                          person.LastName,
                                                          person.IdentificationNumber,
                                                          person.BirthDate,
                                                          person.CityId,
                                                          person.FilePath,
                                                          person.PhotoHeight,
                                                          person.PhotoWidth,
                                                          person.Gender,
                                                          relatedPersons);

                return await OkAsync(result);
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    public class PersonDetailsQueryResult
    {
        public int AggregateRootId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string IdentificationNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public int CityId { get; private set; }

        public string FilePath { get; private set; }

        public int PhotoHeight { get; private set; }

        public int PhotoWidth { get; private set; }

        public string Gender { get; private set; }

        public IEnumerable<RelatedPerson> RelatedPersons { get; private set; }

        public PersonDetailsQueryResult(int aggregateRootId,
                                        string firstName,
                                        string lastName,
                                        string identificationNumber,
                                        DateTime birthDate,
                                        int cityId,
                                        string filePath,
                                        int photoHeight,
                                        int photoWidth,
                                        string gender,
                                        IEnumerable<RelatedPerson> relatedPersons)
        {
            AggregateRootId = aggregateRootId;
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            BirthDate = birthDate;
            CityId = cityId;
            FilePath = filePath;
            PhotoHeight = photoHeight;
            PhotoWidth = photoWidth;
            Gender = gender;
            RelatedPersons = relatedPersons;
        }
    }

    public class RelatedPerson
    {
        public int PersonId { get; private set; }

        public int SecondPersonId { get; private set; }

        public int RelationshipType { get; private set; }

        [JsonIgnore]
        public string Relationship { get; private set; }

        public RelatedPerson(int personId,
                             int secondPersonId,
                             int relationshipType)
        {
            PersonId = personId;
            SecondPersonId = secondPersonId;
            RelationshipType = relationshipType;
            Relationship = ((RelationshipType)relationshipType).ToString();
        }
    }
}
