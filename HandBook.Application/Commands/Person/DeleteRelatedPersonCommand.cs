using System;
using Newtonsoft.Json;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Attributes;
using Microsoft.EntityFrameworkCore;
using HandBook.Application.Infrastructure;
using HandBook.Application.Queries.Person;
using HandBook.Domain.PersonManagement.ReadModels;

namespace HandBook.Application.Commands.Person
{
    [Validator(typeof(DeleteRelatedPersonCommandValidator))]
    public class DeleteRelatedPersonCommand : Command
    {
        public int PersonId { get; set; }
        public int RelatedPersonId { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var relatedPerson = await _db.Set<Domain.PersonManagement.RelatedPerson>()
                                         .FirstOrDefaultAsync(relatedPerson => relatedPerson.RelatedPersonId == RelatedPersonId &&
                                                              relatedPerson.PersonId == PersonId);
            if (relatedPerson == null)
                return await FailAsync(ErrorCode.NotFound);

            _db.Set<Domain.PersonManagement.RelatedPerson>()
               .Remove(relatedPerson);

            var personReadModel = await _db.Set<PersonReadModel>()
                                           .FirstOrDefaultAsync(readModel => readModel.AggregateRootId == PersonId);

            if (personReadModel != null && !string.IsNullOrWhiteSpace(personReadModel.RelatedPersonJson))
            {
                var personRelationships = JsonConvert.DeserializeObject<IEnumerable<RelatedPerson>>(personReadModel.RelatedPersonJson);

                if (personRelationships?.Any() ?? true)
                {
                    personRelationships = personRelationships.Where(rel => rel.RelatedPersonId != RelatedPersonId);

                    var personRelationshipsJson = JsonConvert.SerializeObject(personRelationships);
                    personReadModel.ChangeRelatedPersonJson(personRelationshipsJson);

                    _db.Set<PersonReadModel>().Update(personReadModel);
                }
            }

            await _unitOfWork.SaveAsync();

            return await OkAsync(new DomainOperationResult());
        }
    }

    internal class DeleteRelatedPersonCommandValidator : AbstractValidator<DeleteRelatedPersonCommand>
    {
        public DeleteRelatedPersonCommandValidator()
        {
            RuleFor(person => person.PersonId)
                .GreaterThan(0)
                .WithMessage("PersonId should be greater than zero.");

            RuleFor(person => person.RelatedPersonId)
                .GreaterThan(0)
                .WithMessage("RelatedPersonId should be greater than zero.");
        }
    }
}
