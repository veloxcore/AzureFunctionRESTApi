using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Rest.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void CommitTransaction();
        void StartTransaction();
        void RollBackTransaction();
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        private readonly AppDbContext _appDataContext;
        public DbContext DatabaseContext => _appDataContext;

        public UnitOfWork(AppDbContext appDataContext)
        {
            _appDataContext = appDataContext;
        }

        #region Public Methods
        public void CommitTransaction()
        {
            SaveChanges();
            _transaction.Commit();
            _transaction.Dispose();
        }
        public void StartTransaction()
        {
            _transaction = DatabaseContext.Database.BeginTransaction();
        }
        public void RollBackTransaction()
        {
            _transaction.Rollback();
        }
        public void SaveChanges()
        {
            _appDataContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _appDataContext.SaveChangesAsync();
        }

        #endregion

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DatabaseContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
