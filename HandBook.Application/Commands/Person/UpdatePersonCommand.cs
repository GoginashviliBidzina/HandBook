using System;
using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Attributes;
using HandBook.Application.Helpers;
using HandBook.Domain.PersonManagement;
using HandBook.Application.Infrastructure;
using HandBook.Application.Commands.Person.Models;

namespace HandBook.Application.Commands.Person
{
    [Validator(typeof(UpdatePersonCommandValidator))]
    public class UpdatePersonCommand : Command
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirDateTime { get; set; }

        public int CityId { get; set; }

        public int PhotoId { get; set; }

        public IList<PhoneNumberDto> PhoneNumber { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var isIdentificationInUse = _personRepository
                    .Query(person => person.IdentificationNumber == IdentificationNumber &&
                                              person.Id != Id)
                    .Any();

            if (isIdentificationInUse)
                return await FailAsync(ErrorCode.PhoneNumberInUse);

            var isValidCity = _cityRepository.Query(city => city.Id == CityId).Any();

            if (!isValidCity)
                return await FailAsync(ErrorCode.CityNotFound);

            var person = await _personRepository.GetByIdAsync(Id);

            if (person == null)
                return await FailAsync(ErrorCode.NotFound);

            var photo = await _photoRepository.GetByIdAsync(PhotoId);
            var photoValueObject = new Domain.PersonManagement.ValueObjects.Photo(photo.FilePath,
                                                                                  photo.Width,
                                                                                  photo.Height);

            var phoneNumbers = person.PhoneNumbers.ToList();
            foreach (var item in PhoneNumber)
            {
                var phoneNumber = phoneNumbers.FirstOrDefault(number => number.PhoneNumberType == item.PhoneNumberType);

                if (phoneNumber == null)
                    phoneNumbers.Add(new PhoneNumber(item.Number,
                                                         item.PhoneNumberType));
                else
                {
                    var duplicate = await _personRepository.GetPhoneNumberAsync(item.Number);

                    if (duplicate != null && duplicate.PersonId != Id)
                        return await FailAsync(ErrorCode.PhoneNumberInUse);
                    else
                    {
                        phoneNumber.ChangeNumber(item.Number);
                        phoneNumbers.Add(phoneNumber);
                    }
                }
            }

            person.ChangeDetails(FirstName,
                                LastName,
                                IdentificationNumber,
                                BirDateTime,
                                CityId,
                                photoValueObject,
                                Gender,
                                phoneNumbers);

            await SaveAsync(person, _personRepository);

            return await OkAsync(new DomainOperationResult(person.Id));
        }
    }

    internal class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(person => person.FirstName)
                .Length(2, 50)
                .WithMessage("Please fill first name field.")
                .SetValidator(new LanguageValidation());

            RuleFor(person => person.LastName)
                .Length(2, 50)
                .WithMessage("Please fill last name field.")
                .SetValidator(new LanguageValidation());

            RuleFor(person => person.IdentificationNumber)
                .Length(11)
                .WithMessage("IdentificationNumber length should not be greater or less than 11.");

            RuleFor(person => person.CityId)
                .GreaterThan(0);

            RuleFor(person => person.BirDateTime)
                .LessThanOrEqualTo(DateTime.Now.AddYears(-18))
                .WithMessage("Person's age should be equal or greater than 18 years.");

            RuleFor(person => person.Gender)
                .IsInEnum()
                .WithMessage("Incorrect gender type, please choose gender from select box.");

            RuleForEach(person => person.PhoneNumber)
                .SetValidator(phoneNumber => new PhoneNumberDtoValidator());
        }
    }
}
