using HandBook.Infrastructure.Shared;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.PhotoManagement.Repositories;

namespace HandBook.Infrastructure.Repositories.Photo
{
    public class PhotoRepository : BaseRepository<DatabaseContext, Domain.PhotoManagement.Photo>, IPhotoRepository
    {
        DatabaseContext _context;

        public PhotoRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
