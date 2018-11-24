using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.Data.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        TEntity GetByID(object id);
        Task<TEntity> GetByIDAsync(object id);

        void Insert(TEntity entity);

        void Delete(object id);
        Task DeleteAsync(object id);
        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
        Task UpdateAsync(TEntity entityToUpdate);
    }

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public DbSet<TEntity> dbSet;
        protected internal DbContext DatabaseContext => ((UnitOfWork)_unitOfWork).DatabaseContext;

        public Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }
            _unitOfWork = unitOfWork;
            this.dbSet = DatabaseContext.Set<TEntity>();
        }

        #region Public Methods

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }


        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }


        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }


        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }
        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            dbSet.Remove(entityToDelete);
        }


        public virtual void Delete(TEntity entityToDelete)
        {
            if (DatabaseContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            EntityEntry<TEntity> entry = DatabaseContext.Entry<TEntity>(entityToUpdate);

            if (entry.State == EntityState.Detached)
            {
                TEntity attachedEntity = dbSet.Find(GetKey(entityToUpdate));

                // You need to have access to key
                if ((attachedEntity != null))
                {
                    dynamic attachedEntry = DatabaseContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                }
                else
                {
                    // This should attach entity
                    entry.State = EntityState.Modified;
                }
            }
        }
        public virtual async Task UpdateAsync(TEntity entityToUpdate)
        {
            EntityEntry<TEntity> entry = DatabaseContext.Entry<TEntity>(entityToUpdate);

            if (entry.State == EntityState.Detached)
            {
                TEntity attachedEntity = await dbSet.FindAsync(GetKey(entityToUpdate));

                // You need to have access to key
                if ((attachedEntity != null))
                {
                    dynamic attachedEntry = DatabaseContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                }
                else
                {
                    // This should attach entity
                    entry.State = EntityState.Modified;
                }
            }
        }

        #endregion

        #region Abstract Methods

        protected abstract object[] GetKey(TEntity entity);

        #endregion

        #region "internal methods"

        internal virtual IQueryable<TEntity> Query()
        {
            return dbSet.AsQueryable();
        }

        #endregion
    }
}
