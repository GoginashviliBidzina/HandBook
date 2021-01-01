using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HandBook.Application.Infrastructure;

namespace HandBook.Application.Commands.Person
{
    [Validator(typeof(DeletePersonCommandValidator))]
    public class DeletePersonCommand : Command
    {
        public int Id { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var person = await _personRepository.GetByIdAsync(Id);

            if (person == null)
                return await FailAsync(ErrorCode.NotFound);

            person.RaiseDeletedEvent();

            _personRepository.Delete(person);

            await _unitOfWork.SaveAsync();

            return await OkAsync(new DomainOperationResult());
        }
    }

    internal class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator()
        {
            RuleFor(person => person.Id)
                .GreaterThan(0)
                .WithMessage("Id should be greater than zero.");
        }
    }
}
