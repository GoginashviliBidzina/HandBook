using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HandBook.Domain.PersonManagement;
using HandBook.Application.Infrastructure;

namespace HandBook.Application.Queries.Person
{
    public class PersonReportQuery : Query<IEnumerable<PersonReportQueryResult>>
    {
        public RelationshipType RelationshipType { get; set; }

        public async override Task<QueryExecutionResult<IEnumerable<PersonReportQueryResult>>> ExecuteAsync()
        {
            var relationships = await _db.Set<Domain.PersonManagement.RelatedPerson>()
                                                         .Where(relatedPerson => relatedPerson.RelationshipType == RelationshipType)
                                                         .ToListAsync();

            var groupedRelationships = relationships.GroupBy(relatedPerson => relatedPerson.PersonId)
                                                                    .ToDictionary(x => x.Key, y => y.Count());

            var result = groupedRelationships.Select(grouped => new PersonReportQueryResult(grouped.Key,
                                                                                                                                   grouped.Value));
            return await OkAsync(result);
        }
    }

    public class PersonReportQueryResult
    {
        public int PersonId { get; private set; }

        public int RelationshipsCount { get; private set; }

        public PersonReportQueryResult(int personId,
                                       int relationshipsCount)
        {
            PersonId = personId;
            RelationshipsCount = relationshipsCount;
        }
    }
}
