using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.GenericRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string includeProperties = "");
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, string includeProperties = "");
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeProperties = "");

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity); 
        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);
    }
}
