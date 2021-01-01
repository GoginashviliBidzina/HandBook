using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandBook.Infrastructure.DataBase.Configuration.Person
{
    public class PersonReadModelConfiguration : IEntityTypeConfiguration<Domain.PersonManagement.ReadModels.PersonReadModel>
    {
        public void Configure(EntityTypeBuilder<Domain.PersonManagement.ReadModels.PersonReadModel> builder)
            => builder.ToTable(nameof(Domain.PersonManagement.ReadModels.PersonReadModel));
    }
}
