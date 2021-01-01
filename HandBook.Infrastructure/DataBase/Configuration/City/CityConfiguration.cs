using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandBook.Infrastructure.DataBase.Configuration.City
{
    public class CityConfiguration : IEntityTypeConfiguration<Domain.CityManagement.City>
    {
        public void Configure(EntityTypeBuilder<Domain.CityManagement.City> builder)
             => builder.ToTable(nameof(Domain.CityManagement.City));
    }
}
