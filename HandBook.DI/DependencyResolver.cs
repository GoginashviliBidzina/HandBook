using Microsoft.EntityFrameworkCore;
using HandBook.Domain.PersonManagement;
using HandBook.Infrastructure.DataBase;
using Microsoft.Extensions.Configuration;
using HandBook.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using HandBook.Infrastructure.EventDispatching;
using HandBook.Infrastructure.Repositories.City;
using HandBook.Infrastructure.Repositories.Photo;
using HandBook.Infrastructure.Repositories.Person;
using HandBook.Domain.CityManagement.Repositories;
using HandBook.Domain.PhotoManagement.Repositories;
using HandBook.Domain.PersonManagement.Repositories;

namespace HandBook.DI
{
    public class DependencyResolver
    {
        private IConfiguration _configuration { get; }

        public DependencyResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection Resolve(IServiceCollection services)
        {
            services ??= new ServiceCollection();

            var connectionString = _configuration.GetConnectionString("HandBookDbContext");

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString)
                .UseLazyLoadingProxies());

            services.AddScoped(provider => new InternalDomainEventDispatcher(
                services.BuildServiceProvider(),
                typeof(Person).Assembly,
                typeof(Command).Assembly));

            services.AddScoped<CommandExecutor>();
            services.AddScoped<QueryExecutor>();
            services.AddScoped<UnitOfWork>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            return services;
        }
    }
}
