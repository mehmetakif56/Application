using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DBE.ENERGY.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                             string includeProperties = null,
                             bool asNoTracking = false,
                             int? skip = null,
                             int? take = null
                             );

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            string includeProperties = null,
                            bool asNoTracking = false,
                            int? skip = null,
                            int? take = null
                            );

        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            string includeProperties = null);

        TEntity GetById(object id);

        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null, bool asNoTracking = false);

        void Create(TEntity entity, Guid? userId = null);

        void Create(System.Guid? userId = null, params TEntity[] entities);

        void Create(IEnumerable<TEntity> entities, Guid? userId = null);

        void Update(TEntity entity, Guid? userId = null);

        void Update(Guid? userId = null, params TEntity[] entities);

        void Update(IEnumerable<TEntity> entities, Guid? userId = null, bool bulkUpdate = false);

        void Delete(TEntity entity);

        void Delete(params TEntity[] entities);

        void Delete(IEnumerable<TEntity> entities);

        void Delete(object id);

        bool Save();

        IEnumerable<TEntity> FromSql(string query, params object[] param);
    }

}
