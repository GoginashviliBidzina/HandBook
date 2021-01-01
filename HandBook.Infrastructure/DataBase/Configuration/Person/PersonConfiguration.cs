using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandBook.Infrastructure.DataBase.Configuration.Person
{
    public class PersonConfiguration : IEntityTypeConfiguration<Domain.PersonManagement.Person>
    {
        public void Configure(EntityTypeBuilder<Domain.PersonManagement.Person> builder)
        {
            builder.OwnsOne(person => person.Photo);
            builder.HasMany(person => person.RelatedPersons);
            builder.HasMany(person => person.PhoneNumbers);

            builder.ToTable(nameof(Domain.PersonManagement.Person));
        }
    }
}
