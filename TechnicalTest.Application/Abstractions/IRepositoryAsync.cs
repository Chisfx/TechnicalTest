using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace TechnicalTest.Application.Interfaces.Abstractions
{
    public interface IRepositoryAsync<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> GetByIdAsync<TKey>(TKey Id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(List<T> entities);
    }
}
