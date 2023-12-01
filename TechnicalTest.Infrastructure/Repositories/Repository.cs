using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechnicalTest.Application.Interfaces.Repositories;
using TechnicalTest.Infrastructure.DbContexts;
namespace TechnicalTest.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SecurityDbContext _dbContext;
        public Repository(SecurityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> AddAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if(includes != null) query = includes(query).AsNoTracking();
            return where != null ? await query.AnyAsync(where) : await query.AnyAsync();
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = where != null ? _dbContext.Set<T>().Where(where).AsNoTracking() : _dbContext.Set<T>().AsNoTracking();
            if (includes != null) query = includes(query).AsNoTracking();
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync<TKey>(TKey Id)
        {
            var result = await _dbContext.Set<T>().FindAsync(Id);
            return result;
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = where != null ? _dbContext.Set<T>().Where(where).AsNoTracking() : _dbContext.Set<T>().AsNoTracking();
            if (includes != null) query = includes(query).AsNoTracking();
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
    }
}
