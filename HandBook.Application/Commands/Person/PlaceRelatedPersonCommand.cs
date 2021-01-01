using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Attributes;
using HandBook.Domain.PersonManagement;
using HandBook.Application.Infrastructure;
using HandBook.Application.Commands.Person.Models;

namespace HandBook.Application.Commands.Person
{
    [Validator(typeof(PlaceRelatedPersonCommandValidator))]
    public class PlaceRelatedPersonCommand : Command
    {
        public int PersonId { get; set; }

        public IEnumerable<RelatedPersonDto> RelatedPersons { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var person = await _personRepository.GetByIdAsync(PersonId);

            if (person == null)
                return await FailAsync(ErrorCode.NotFound);

            var relatedPersons = RelatedPersons.Select(relatedPerson => new RelatedPerson(PersonId,
                                                                                                                relatedPerson.RelatedPersonId,
                                                                                                                relatedPerson.RelationshipType));

            person.SetRelatedPerson(relatedPersons.ToList());

            await SaveAsync(person, _personRepository);

            return await OkAsync(new DomainOperationResult());
        }
    }

    internal class PlaceRelatedPersonCommandValidator : AbstractValidator<PlaceRelatedPersonCommand>
    {
        public PlaceRelatedPersonCommandValidator()
        {
            RuleFor(relatedPerson => relatedPerson.PersonId)
                .GreaterThan(0);
        }
    }
}
