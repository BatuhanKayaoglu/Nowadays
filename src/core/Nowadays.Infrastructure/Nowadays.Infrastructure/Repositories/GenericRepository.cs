using Nowadays.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Nowadays.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Repositories
{
    // The reason we use virtual is so that we can override, that is, change the methods here in other repositories if necessary.
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext dbContext;

        protected DbSet<TEntity> entity => dbContext.Set<TEntity>();

        public GenericRepository(DbContext dbContext) // We give DbContext instead of Nowadays Context so that we can use it in other db etc. in the future.
        {
            this.dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        #region Insert Methods
        public virtual void Add(TEntity entity)
        {
            this.entity.Add(entity);
            dbContext.SaveChangesAsync();

        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            entity.AddRange(entities);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task AddAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await entity.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();


        }

        #endregion



        #region Update Methods
        public virtual void Update(TEntity entity)
        {
            this.entity.Attach(entity); //By enabling the entity object to be tracked, it allows changes to be managed more effectively on the Entity Framework.
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChangesAsync();


        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

        }

        #endregion




        #region Delete Methods
        public virtual void Delete(TEntity entity)
        {
            // If, after making changes to an entity that is disconnected from the context, this entity is deleted from the database and 'Attach' is not made,
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);
            dbContext.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);
            await dbContext.SaveChangesAsync();

        }

        public virtual void Delete(Guid id)
        {
            var entity = this.entity.Find(id);
            Delete(entity);
            dbContext.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = this.entity.Find(id);
            await DeleteAsync(entity);
            await dbContext.SaveChangesAsync();

        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
        }

        public virtual async Task DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(predicate);

        }

        #endregion


        #region AddOrUpdate Methods

        public virtual void AddOrUpdate(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);
        }

        public virtual async Task AddOrUpdateAsync(TEntity entity)
        {
            // chech the entity with the id already tracked
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);
        }
        #endregion



        #region Get methods
        public IQueryable<TEntity> AsQueryable() => entity.AsQueryable();

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {

            /*  
             *  includes, sorgunun yürütülmesi sırasında ilgili nesnelerin (ilişkili nesnelerin) yüklenmesini sağlar. Bu, "eager loading" olarak bilinir ve performansı artırabilir.
             *  Birden fazla Expression<Func<TEntity, object>> ifadesi alabilir, bu sayede sorguya ilişkili tabloların (ilişkili nesnelerin) eklenmesini sağlar.
             */
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }


        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }


        public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            IQueryable<TEntity> query = entity;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity? found = await entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                dbContext.Entry(found).State = EntityState.Detached;

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                dbContext.Entry(found).Reference(include).Load();
            }

            return found;
        }



        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }

        #endregion



        #region Bulk Methods

        public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                return Task.CompletedTask;

            dbContext.RemoveRange(entity.Where(i => ids.Contains(i.Id)));
            return Task.CompletedTask;
        }
        public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            await entity.AddRangeAsync(entities);
        }

        public virtual async Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = dbContext.Set<TEntity>().Where(predicate);
            dbContext.RemoveRange(entitiesToDelete);
        }

        public virtual async Task BulkDelete(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            dbContext.RemoveRange(entities);
        }

        public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                return Task.CompletedTask;

            foreach (var entity in entities)
            {
                // Attach the entity to the DbContext if not already attached
                if (!dbContext.Set<TEntity>().Local.Contains(entity))
                    dbContext.Set<TEntity>().Attach(entity);

                // Mark the entity as modified to ensure it gets updated
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }

        #endregion

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            #region Fonksiyonun kullanımı
            /*
             *  Bu metodun adı "ApplyIncludes" ve bu metot, bir IQueryable sorgusuna "eager loading" yapmak amacıyla ilişkili nesneleri eklemek için kullanılıyor. "Eager loading", ilişkili nesnelerin, ana nesneyle birlikte bir sorgu ile yüklenmesini ifade eder. Bu, daha sonra ilişkili nesnelere erişim sağlamak için ek bir sorgu yapma ihtiyacını ortadan kaldırabilir ve performans avantajı sağlayabilir.

                query (IQueryable<TEntity> query):

                Bu parametre, LINQ sorgusunun temelini oluşturan IQueryable nesnesini temsil eder. Bu genellikle bir veritabanı tablosundan veri çeken bir LINQ sorgusudur.
                includes (params Expression<Func<TEntity, object>>[] includes):

                includes parametresi, ilişkili nesnelerin (ilişkili tabloların) belirtilen property'lerini içeren bir dizi lambda ifadesini temsil eder.
                Bu lambda ifadeleri, sorguya eklenmiş olarak belirtilen property'lerin (ilişkili nesnelerin) yüklenmesini sağlar.

                Metodun İşleyişi:
                ApplyIncludes metodu, foreach döngüsü ile includes parametresinde belirtilen lambda ifadelerini alır.
                Her bir lambda ifadesi, query.Include(include) kullanılarak, sorguya belirtilen ilişkili nesnelerin yüklenmesini ekler.
                Bu işlem, LINQ sorgusunu genişleterek, belirtilen ilişkili nesnelerin sorgu sonucuna dahil edilmesini sağlar.
                Son olarak, genişletilmiş sorgu olan query nesnesi geri döndürülür.
                Bu metodun amacı, query üzerine belirtilen ilişkili nesnelerin yüklenmesini eklemektir. Bu sayede, sorgunun sonucu, ilişkili nesnelerle birlikte alınabilir ve daha sonra bu nesnelere erişim sağlamak için ek bir sorgu yapma ihtiyacı ortadan kalkar.
             */
            #endregion

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }



    }
}
