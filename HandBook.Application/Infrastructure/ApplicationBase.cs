using System;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.CityManagement.Repositories;
using HandBook.Domain.PhotoManagement.Repositories;
using HandBook.Domain.PersonManagement.Repositories;

namespace HandBook.Application.Infrastructure
{
    public abstract class ApplicationBase
    {
        protected DatabaseContext _db;
        protected UnitOfWork _unitOfWork;
        protected IServiceProvider _serviceProvider;
        protected ICityRepository _cityRepository;
        protected IPhotoRepository _photoRepository;
        protected IPersonRepository _personRepository;

        public void Resolve(DatabaseContext db,
                            UnitOfWork unitOfWork,
                            IServiceProvider serviceProvider,
                            ICityRepository cityRepository, 
                            IPhotoRepository photoRepository,
                            IPersonRepository personRepository)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _cityRepository = cityRepository;
            _photoRepository = photoRepository;
            _personRepository = personRepository;
        }

        public T GetService<T>() => (T)_serviceProvider.GetService(typeof(T));
    }
}
