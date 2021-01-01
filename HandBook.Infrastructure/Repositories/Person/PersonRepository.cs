using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HandBook.Infrastructure.Shared;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.PersonManagement;
using HandBook.Domain.PersonManagement.Repositories;

namespace HandBook.Infrastructure.Repositories.Person
{
    public class PersonRepository : BaseRepository<DatabaseContext, Domain.PersonManagement.Person>, IPersonRepository
    {
        DatabaseContext _context;

        public PersonRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PhoneNumber> GetPhoneNumberAsync(string number)
            => await _context.Set<PhoneNumber>()
                             .FirstOrDefaultAsync(phoneNumber => phoneNumber.Number == number);
    }
}
