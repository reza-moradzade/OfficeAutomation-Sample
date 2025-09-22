// Generic repository base class providing standard CRUD operations for any entity
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Data.Context;

namespace OfficeAutomation.Repositories.Base
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        // EF DbContext instance
        protected readonly OfficeAutomationDbContext _context;

        // DbSet for the entity type T
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(OfficeAutomationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Get entity by primary key
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Find entities by a predicate
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Add a new entity
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Update an existing entity
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Remove an entity
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
