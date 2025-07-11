using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal TaskManagementContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(TaskManagementContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        private IQueryable<T> GetIncludeProperties(IQueryable<T> query, string includeProperties = "")
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
            return query;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<T> query = GetIncludeProperties(dbSet, includeProperties);
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            IQueryable<T> query = GetIncludeProperties(dbSet, includeProperties);
            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            IQueryable<T> query = GetIncludeProperties(dbSet, includeProperties);
            return await query.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual Task<int> CountAsync()
        {
            return dbSet.CountAsync();
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate) 
        { 
            return dbSet.CountAsync(predicate); 
            }

        public virtual Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            return dbSet.AnyAsync(predicate);
        }
    }
}
