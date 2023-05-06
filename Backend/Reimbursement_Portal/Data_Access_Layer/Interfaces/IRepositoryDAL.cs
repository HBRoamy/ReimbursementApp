using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Interfaces
{
    public interface IRepositoryDAL<TEntity> where TEntity:class
    {
        public IEnumerable<TEntity> All();
        public Task<IEnumerable<TEntity>> AllAsync();
        public TEntity GetById(object id);
        public Task<TEntity> GetByIdAsync(object id);
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
        public bool Contains(Expression<Func<TEntity, bool>> predicate);
        public TEntity Find(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        public TEntity Create(TEntity entity);
        public Task<TEntity> CreateAsync(TEntity entity);
        public void Delete(object id);
        public Task<bool> DeleteAsync(object id);
        public void Delete(TEntity entity);
        public void Delete(Expression<Func<TEntity, bool>> predicate);

        public Task SaveAsync();
    }
}
