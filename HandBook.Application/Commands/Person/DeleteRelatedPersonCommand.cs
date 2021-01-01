using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Microsoft.EntityFrameworkCore;
using HandBook.Application.Infrastructure;

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

            _db.Set<Domain.PersonManagement.RelatedPerson>()
               .Remove(relatedPerson);

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
