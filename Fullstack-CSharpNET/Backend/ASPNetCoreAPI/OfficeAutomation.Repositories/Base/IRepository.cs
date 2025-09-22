using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Base
{
    // Generic repository interface for CRUD operations
    public interface IRepository<T> where T : class
    {
        // Get entity by primary key
        Task<T?> GetByIdAsync(int id);

        // Get all entities
        Task<IEnumerable<T>> GetAllAsync();

        // Find entities by a predicate
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add new entity
        Task AddAsync(T entity);

        // Update existing entity
        void Update(T entity);

        // Remove entity
        void Remove(T entity);
    }
}
