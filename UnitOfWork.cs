using DBE.ENERGY.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBE.ENERGY.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed;
        private Dictionary<Type, object> _repositories { get; set; }
        private readonly DBEEnergyContext _context;
        public DbContext Context { get => _context; set { } }

        public UnitOfWork(DBEEnergyContext context) => _context = context;

        public bool Complete() => Convert.ToBoolean(_context.SaveChanges());

        public int ExecuteSqlCommand(string sql, params object[] param) => _context.Database.ExecuteSqlCommand(sql, param);       

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new EFRepository<TEntity>((DBEEnergyContext)Context);

            return (IRepository<TEntity>)_repositories[type];
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (_repositories != null)
                    {
                        _repositories.Clear();
                    }
                    // dispose the db context.
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] param) where TEntity : class
            => _context.Set<TEntity>().FromSqlRaw(sql, param);
        #endregion
    }
}
