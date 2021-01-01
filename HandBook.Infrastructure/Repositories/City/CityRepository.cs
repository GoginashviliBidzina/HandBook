using HandBook.Infrastructure.Shared;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.CityManagement.Repositories;

namespace HandBook.Infrastructure.Repositories.City
{
    public class CityRepository : BaseRepository<DatabaseContext, Domain.CityManagement.City>, ICityRepository
    {
        DatabaseContext _context;

        public CityRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
