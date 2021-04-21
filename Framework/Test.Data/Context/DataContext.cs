#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Test.Data.Context
{
    public class DataContext : DbContext, IDataContext
    {
        private readonly DbContextId _instanceId;
        private readonly DatabaseFacade _Database;

        public DataContext()
        {

        }
        public DataContext(DbContextOptions connectionOptions)
            : base(connectionOptions)
        {
            _instanceId = base.ContextId;
            _Database = base.Database;

            ChangeTracker.LazyLoadingEnabled = false;
        }

        public override DbContextId ContextId
        {
            get { return _instanceId; }
        }
        public override DatabaseFacade Database
        {
            get { return _Database; }
        }
        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).ObjectState);
        }

        

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
        }

        private void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
        }
    }
}