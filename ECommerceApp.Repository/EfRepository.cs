using ECommerceApp.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ECommerceApp.Repository
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EfRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving entities: {ex.Message}", ex);
            }
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID cannot be empty.");

                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving entity by ID: {ex.Message}", ex);
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding entity: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating entity: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID cannot be empty.");

                var entity = await GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Entity with ID '{id}' not found.");

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity: {ex.Message}", ex);
            }
        }
    }
}