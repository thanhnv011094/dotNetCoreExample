using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace example.DataProvider.Repositories
{
    public class Reponsitory<T> : IReponsitory<T> where T : class
    {
        private readonly DbSet<T> _entities;
        public Reponsitory(DbContext dbContext)
        {
            _entities = dbContext.Set<T>();
        }

        public async void Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async void AddRange(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToArrayAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToArrayAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
