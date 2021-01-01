using Microsoft.EntityFrameworkCore;
using HandBook.Infrastructure.DataBase.Configuration.City;
using HandBook.Infrastructure.DataBase.Configuration.Photo;
using HandBook.Infrastructure.DataBase.Configuration.Person;

namespace HandBook.Infrastructure.DataBase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CityConfiguration());

            builder.ApplyConfiguration(new PhotoConfiguration());

            builder.ApplyConfiguration(new PersonConfiguration());
            builder.ApplyConfiguration(new PersonReadModelConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
