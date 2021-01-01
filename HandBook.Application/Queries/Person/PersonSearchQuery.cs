using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HandBook.Domain.PersonManagement;
using HandBook.Application.Infrastructure;
using HandBook.Domain.PersonManagement.ReadModels;

namespace HandBook.Application.Queries.Person
{
    public class PersonSearchQuery : Query<PersonSearchQueryResult>
    {
        public string Key { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public async override Task<QueryExecutionResult<PersonSearchQueryResult>> ExecuteAsync()
        {
            var persons = await _db.Set<PersonReadModel>()
                                                     .Where(person => EF.Functions.Like(person.FirstName, $"%{Key}%") ||
                                                                      EF.Functions.Like(person.LastName, $"%{Key}%") ||
                                                                      EF.Functions.Like(person.IdentificationNumber, $"%{Key}%"))
                                                     .Skip(PageSize * PageNumber)
                                                     .Take(PageSize)
                                                     .ToListAsync();

            var result = persons.Select(person => new PersonListing(person.AggregateRootId,
                                                                                          person.FirstName,
                                                                                          person.LastName,
                                                                                          person.IdentificationNumber,
                                                                                          person.BirthDate,
                                                                                          person.CityId,
                                                                                          person.FilePath,
                                                                                          person.PhotoHeight,
                                                                                          person.PhotoWidth,
                                                                                          person.Gender,
                                                                                          JsonConvert.DeserializeObject<IEnumerable<RelatedPersons>>(person.RelatedPersonJson)));

            return await OkAsync(new PersonSearchQueryResult(result.Count(),
                                                                 result));
        }
    }

    public class PersonSearchQueryResult
    {
        public int Count { get; private set; }

        public IEnumerable<PersonListing> PersonsListing { get; private set; }

        public PersonSearchQueryResult(int count,
                                       IEnumerable<PersonListing> personsListing)
        {
            Count = count;
            PersonsListing = personsListing;
        }
    }

    public class PersonListing
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

        public IEnumerable<RelatedPersons> RelatedPersons { get; private set; }

        public PersonListing(int aggregateRootId,
                             string firstName,
                             string lastName,
                             string identificationNumber,
                             DateTime birthDate,
                             int cityId,
                             string filePath,
                             int photoHeight,
                             int photoWidth,
                             string gender,
                             IEnumerable<RelatedPersons> relatedPersons)
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

    public class RelatedPersons
    {
        public int PersonId { get; private set; }

        public int SecondPersonId { get; private set; }

        public int RelationshipType { get; private set; }

        [JsonIgnore]
        public string Relationship { get; private set; }

        public RelatedPersons(int personId,
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
