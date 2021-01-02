using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HandBook.Application.Infrastructure;

namespace HandBook.Application.Commands.City
{
    [Validator(typeof(PlaceCityCommandValidator))]
    public class PlaceCityCommand : Command
    {
        public string Name { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var city = new Domain.CityManagement.City(Name);

            var duplicates = _cityRepository.Query(city => city.Name == Name)
                                                     .ToList();

            if (duplicates?.Any() ?? true)
                return await FailAsync(ErrorCode.DuplicatedCityName);

            await SaveAsync(city, _cityRepository);

            return await OkAsync(new DomainOperationResult(city.Id));
        }
    }

    internal class PlaceCityCommandValidator : AbstractValidator<PlaceCityCommand>
    {
        public PlaceCityCommandValidator()
        {
            RuleFor(city => city.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please, fill city name field.");
        }
    }
}
