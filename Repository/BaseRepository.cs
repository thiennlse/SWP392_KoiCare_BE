using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private KoiCareDBContext _context;

        public BaseRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        protected DbSet<T> _dbSet
        {
            get
            {
                var dbSet = GetDbSet<T>();
                return dbSet;
            }
        }

        protected DbSet<T> GetDbSet<T>() where T : BaseEntity
        {
            var dbSet = _context.Set<T>();
            return dbSet;
        }

        public void add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
        
        public void update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void CheckCancellationToken(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException("Request was cancelled");
        }

        public virtual async Task<T> GetById(int id)
        {
            var queryable = GetQueryable(x => x.Id == id);
            var entity = await queryable.FirstOrDefaultAsync();

            return entity;
        }
        #region GetQueryable
        public IQueryable<T> GetQueryable(CancellationToken cancellationToken = default)
        {
            CheckCancellationToken(cancellationToken);
            var queryable = GetQueryable<T>();
            return queryable;
        }

        public IQueryable<T> GetQueryable<T>() where T : BaseEntity
        {
            IQueryable<T> queryable = GetDbSet<T>();
            return queryable;
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
        {
            var queryable = GetQueryable<T>();
            if (predicate != null) queryable = queryable.Where(predicate);
            return queryable;
        }
        #endregion
    }
}
