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
    public class PersonDetailedSearchQuery : Query<PersonDetailedSearchQueryResult>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int CityId { get; set; }

        public Gender Gender { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public async override Task<QueryExecutionResult<PersonDetailedSearchQueryResult>> ExecuteAsync()
        {
            try
            {
                var persons = await _db.Set<PersonReadModel>()
                                                         .Where(
                                                             person =>
                                                                 (Gender == null ? true : person.Gender == Gender.ToString()) &&
                                                                 (BirthDate == null || BirthDate == default(DateTime) ? true : person.BirthDate == BirthDate) &&
                                                                 (CityId > 0 ? true : person.CityId == CityId) &&
                                                                 (string.IsNullOrWhiteSpace(FirstName) ? true : person.FirstName.Contains(FirstName)) &&
                                                                 (string.IsNullOrWhiteSpace(LastName) ? true : person.LastName.Contains(LastName)) &&
                                                                 (string.IsNullOrWhiteSpace(IdentificationNumber) ? true : person.IdentificationNumber.Contains(IdentificationNumber))
                                                         )
                                                         .Skip(PageSize * PageNumber)
                                                         .Take(PageSize)
                                                         .ToListAsync();

                var result = persons.Select(person => new PersonsListing(person.AggregateRootId,
                                                                                               person.FirstName,
                                                                                               person.LastName,
                                                                                               person.IdentificationNumber,
                                                                                               person.BirthDate,
                                                                                               person.CityId,
                                                                                               person.FilePath,
                                                                                               person.PhotoHeight,
                                                                                               person.PhotoWidth,
                                                                                               person.Gender,
                                                                                               JsonConvert.DeserializeObject<IEnumerable<Relationships>>(person.RelatedPersonJson)));

                return await OkAsync(new PersonDetailedSearchQueryResult(result.Count(),
                                                                             result.ToList()));
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    public class PersonDetailedSearchQueryResult
    {
        public int Count { get; private set; }

        public IEnumerable<PersonsListing> PersonsListing { get; private set; }

        public PersonDetailedSearchQueryResult(int count,
                                               IEnumerable<PersonsListing> personsListing)
        {
            Count = count;
            PersonsListing = personsListing;
        }
    }

    public class PersonsListing
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

        public IEnumerable<Relationships> RelatedPersons { get; private set; }

        public PersonsListing(int aggregateRootId,
                             string firstName,
                             string lastName,
                             string identificationNumber,
                             DateTime birthDate,
                             int cityId,
                             string filePath,
                             int photoHeight,
                             int photoWidth,
                             string gender,
                             IEnumerable<Relationships> relatedPersons)
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

    public class Relationships
    {
        public int PersonId { get; private set; }

        public int SecondPersonId { get; private set; }

        public int RelationshipType { get; private set; }

        [JsonIgnore]
        public string Relationship { get; private set; }

        public Relationships(int personId,
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
