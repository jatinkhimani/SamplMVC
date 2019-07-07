using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AppCustomer.DataAccess;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace RepositoryCore
{
    public class Repository<TEntity> : AbstractRepository<TEntity> where TEntity : class
    {
        private DbContext _dbContext;

        public Repository(CustomerDBEntities context)
        {
            _dbContext = context;
        }

        private DbSet<TEntity> dbSet
        {
            get {
                return _dbContext.Set<TEntity>();
            }
        }
        protected override void Add(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateEx)
            {
                var entry = _dbContext.Entry(entity);
                if (entry.State == EntityState.Added)
                {
                    dbSet.Remove(entity);
                }
                CustomExceptionHandler.ThrowHandledException(dbUpdateEx);
                throw;
            }
            catch(DbEntityValidationException dbEntityEx)
            {
                CustomExceptionHandler.ThrowHandledException(dbEntityEx);
                throw;
            }
        }

        protected override void Delete(object id)
        {
            try
            {
                var entity = Get(id);
                dbSet.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateEx)
            {
                CustomExceptionHandler.ThrowHandledException(dbUpdateEx);
                throw;
            }
        }

        protected override TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).FirstOrDefault();
        }

        protected override List<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }

        protected override TEntity Get(object id)
        {
            return dbSet.Find(id);
        }

        protected override List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        protected override IQueryable<TEntity> SelectQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sort = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (sort != null)
            {
                query = sort(query);
            }
            if (includes != null)
            {
                query = includes.Aggregate(query, (a, b) => a.Include(b));
            }
            return query;
        }

        protected override void Update(TEntity entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateEx)
            {
                CustomExceptionHandler.ThrowHandledException(dbUpdateEx);
                throw;
            }
            catch (DbEntityValidationException dbEntityEx)
            {
                CustomExceptionHandler.ThrowHandledException(dbEntityEx);
                throw;
            }
        }
    }
}
