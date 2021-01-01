using System;
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
    public class QueryExecutor
    {
        protected DatabaseContext _db;
        protected UnitOfWork _unitOfWork;
        protected IServiceProvider _serviceProvider;
        protected ICityRepository _cityRepository;
        protected IPhotoRepository _photoRepository;
        protected IPersonRepository _personRepository;

        public QueryExecutor(DatabaseContext db, UnitOfWork unitOfWork, IServiceProvider serviceProvider, ICityRepository cityRepository, IPhotoRepository photoRepository, IPersonRepository personRepository)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _cityRepository = cityRepository;
            _photoRepository = photoRepository;
            _personRepository = personRepository;
        }

        public async Task<QueryExecutionResult<TResult>> ExecuteAsync<TQuery, TResult>(TQuery query)
            where TQuery : Query<TResult>
            where TResult : class
        {
            try
            {
                var validationResult = Validate(query);

                if (!validationResult.IsValid)
                {
                    return new QueryExecutionResult<TResult>
                    {
                        Success = false,
                        ErrorCode = ErrorCode.ValidationFailed
                    };
                }

                query.Resolve(_db,
                              _unitOfWork,
                              _serviceProvider,
                              _cityRepository,
                              _photoRepository,
                              _personRepository); ;

                return await query.ExecuteAsync();
            }
            catch (Exception)
            {
                return new QueryExecutionResult<TResult>
                {
                    Success = false,
                    ErrorCode = ErrorCode.Exception
                };
            }
        }

        public static ValidationResult Validate<TResult>(Query<TResult> execution) where TResult : class
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
