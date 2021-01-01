using System;
using System.Linq;
using HandBook.Shared;
using System.Threading.Tasks;
using System.Collections.Generic;
using HandBook.Infrastructure.EventDispatching;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HandBook.Infrastructure.DataBase
{
    public class UnitOfWork
    {
        private readonly DatabaseContext _db;
        private readonly InternalDomainEventDispatcher _internalDomainEventDispatcher;

        public UnitOfWork(DatabaseContext db,
                          InternalDomainEventDispatcher internalDomainEventDispatcher)
        {
            _db = db;
            _internalDomainEventDispatcher = internalDomainEventDispatcher;
        }
        public void Save()
        {
            using var transaction = _db.Database.BeginTransaction();
            var modifiedEntries = _db.ChangeTracker.Entries<IHasDomainEvents>().ToList();
            _db.SaveChanges();

            ProcessUncommitedChanges(modifiedEntries);

            _db.SaveChanges();
            transaction.Commit();
        }

        public async Task SaveAsync()
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            var modifiedEntries = _db.ChangeTracker.Entries<IHasDomainEvents>().ToList();
            await _db.SaveChangesAsync();

            ProcessUncommitedChanges(modifiedEntries);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        private void ProcessUncommitedChanges(List<EntityEntry<IHasDomainEvents>> modifiedEntries)
        {
            foreach (var entry in modifiedEntries)
            {
                var events = entry.Entity.UncommittedChanges();
                if (!events.Any()) continue;
                _internalDomainEventDispatcher.Dispatch(events, _db);

                entry.Entity.MarkChangesAsCommitted();
            }
        }
    }
}
