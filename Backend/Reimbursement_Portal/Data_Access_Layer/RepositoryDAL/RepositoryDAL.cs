using Data_Access_Layer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.RepositoryDAL
{
    public class RepositoryDAL<TEntity> : IRepositoryDAL<TEntity> where TEntity : class
    {

        //protected Microsoft.EntityFrameworkCore.DbContext _context; // use this only when you are inheriting from this class
        private readonly Microsoft.EntityFrameworkCore.DbContext _context;
        public RepositoryDAL(Microsoft.EntityFrameworkCore.DbContext context )
        {
            _context = context;
        }



        public IEnumerable<TEntity> All()
        {
            return _context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any(predicate);
        }

        public TEntity Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync(); // without saving can't return id
            return entity;
        }

        public void Delete(object id)
        {
            var entityToDelete = GetById(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = Filter(predicate);
            foreach (var entity in entitiesToDelete)
            {
                //_context.Set<TEntity>().Remove(entity);
                Delete(entity);
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var entityToDelete = await GetByIdAsync(id);
            if (entityToDelete != null)
            {
                _context.Set<TEntity>().Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
        }

        public TEntity GetById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
