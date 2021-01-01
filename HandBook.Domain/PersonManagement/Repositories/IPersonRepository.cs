using HandBook.Shared;
using System.Threading.Tasks;

namespace HandBook.Domain.PersonManagement.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<PhoneNumber> GetPhoneNumberAsync(string number);
    }
}
