using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandBook.Infrastructure.DataBase.Configuration.Photo
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Domain.PhotoManagement.Photo>
    {
        public void Configure(EntityTypeBuilder<Domain.PhotoManagement.Photo> builder)
            => builder.ToTable(nameof(Domain.PhotoManagement.Photo));
    }
}
