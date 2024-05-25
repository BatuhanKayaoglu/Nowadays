using Nowadays.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        Task AddAsync(IEnumerable<TEntity> entities);


        Task UpdateAsync(TEntity entity);
        void Update(TEntity entity);


        Task DeleteAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteAsync(Guid id);
        void Delete(Guid id);
        void DeleteRange(Expression<Func<TEntity, bool>> predicate);
        Task DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);


        Task AddOrUpdateAsync(TEntity entity);
        void AddOrUpdate(TEntity entity);
        IQueryable<TEntity> AsQueryable();
        Task<List<TEntity>> GetAll(bool noTracking = true);
        Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);


        Task BulkDeleteById(IEnumerable<Guid> ids);
        Task BulkDelete(Expression<Func<TEntity, bool>> predicate);
        Task BulkDelete(IEnumerable<TEntity> entities);
        Task BulkUpdate(IEnumerable<TEntity> entities);
        Task BulkAdd(IEnumerable<TEntity> entities);

    }
}
