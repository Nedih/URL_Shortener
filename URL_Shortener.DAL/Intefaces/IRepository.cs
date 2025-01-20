using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace URL_Shortener.DAL.Intefaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetWhere(Func<TEntity, bool> predicate);
        IQueryable<TEntity> GetWhere(Func<TEntity, bool> predicate, Func<TEntity, TEntity> selector);
        TEntity FirstOrDefault(Func<TEntity, bool> predicate);
        int Count(Func<TEntity, bool> predicate);
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Sort(IQueryable<TEntity> items, string props);
        Task SaveAsync();
    }
}
