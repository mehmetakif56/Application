using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DBE.ENERGY.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        DbContext Context { get; set; }

        /// <summary>
        /// Saves all changes in the given context
        /// </summary>
        /// <returns></returns>
        bool Complete();

        /// <summary>
        /// Get repository for given TEntity entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Repository instance of TEntity</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Executes given raw sql command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] param);

        /// <summary>
        /// Executes given raw sql query to fetch <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] param) where TEntity : class;
    }
}
