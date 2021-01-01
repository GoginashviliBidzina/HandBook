using FluentValidation;
using FluentValidation.Attributes;
using HandBook.Domain.PersonManagement;

namespace HandBook.Application.Commands.Person.Models
{
    [Validator(typeof(PhoneNumberDtoValidator))]
    public class PhoneNumberDto
    {
        public string Number { get; set; }

        public PhoneNumberType PhoneNumberType { get; set; }
    }

    internal class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
    {
        public PhoneNumberDtoValidator()
        {
            RuleFor(phoneNumber => phoneNumber.Number)
                .Length(4, 50)
                .WithMessage("Phone number should be greater than 4 and less than 50 characters.");

            RuleFor(phoneNumber => phoneNumber.PhoneNumberType)
                .IsInEnum()
                .WithMessage("Incorrect phone number type.");
        }
    }
}
