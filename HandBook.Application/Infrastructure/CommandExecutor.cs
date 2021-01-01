using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation.Attributes;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.CityManagement.Repositories;
using HandBook.Domain.PersonManagement.Repositories;
using HandBook.Domain.PhotoManagement.Repositories;

namespace HandBook.Application.Infrastructure
{
    public class CommandExecutor
    {
        protected DatabaseContext _db;
        protected UnitOfWork _unitOfWork;
        protected IServiceProvider _serviceProvider;
        protected ICityRepository _cityRepository;
        protected IPhotoRepository _photoRepository;
        protected IPersonRepository _personRepository;

        public CommandExecutor(DatabaseContext db, UnitOfWork unitOfWork, IServiceProvider serviceProvider, ICityRepository cityRepository, IPhotoRepository photoRepository, IPersonRepository personRepository)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _cityRepository = cityRepository;
            _photoRepository = photoRepository;
            _personRepository = personRepository;   
        }

        public async Task<CommandExecutionResult> ExecuteAsync(Command command)
        {
            try
            {
                var validationResult = Validate(command);

                if (!validationResult.IsValid)
                {
                    return new CommandExecutionResult
                    {
                        Success = false,
                        ErrorCode = ErrorCode.ValidationFailed,
                        Errors = validationResult.Errors.Select(error => error.ErrorMessage)
                    };
                }

                command.Resolve(_db,
                                _unitOfWork,
                                _serviceProvider,
                                _cityRepository,
                                _photoRepository,
                                _personRepository);

                return await command.ExecuteAsync();
            }
            catch (Exception)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorCode = ErrorCode.Exception
                };
            }
        }

        public static ValidationResult Validate(Command execution)
        {
            var validatorAttribute = execution.GetType().GetCustomAttribute<ValidatorAttribute>(true);
            if (validatorAttribute != null)
            {
                var instance = (dynamic)Activator.CreateInstance(validatorAttribute.ValidatorType);
                var modelState = instance.Validate((dynamic)execution);
                return modelState;
            }

            return new ValidationResult();
        }
    }
}
