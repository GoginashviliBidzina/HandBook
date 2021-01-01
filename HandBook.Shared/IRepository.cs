using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HandBook.Shared
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : class
    {
        Task<TAggregateRoot> GetByIdAsync(int id);

        void Delete(TAggregateRoot aggregateRoot);

        void Save(TAggregateRoot aggregateRoot);

        IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> expression = null);
    }
}
