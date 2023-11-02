using System.Linq.Expressions;
using HotelManagment_API.Data;
using HotelManagment_API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagment_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private protected readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? includeProperties = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool isTracked = true,
            int pageNumber = 0,
            int pageSize = 0
        )
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (!isTracked)
            {
                query = query.AsNoTracking();
            }

            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? includeProperties = null,
            bool isTracked = true
        )
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            if (!isTracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                return await query.FirstOrDefaultAsync(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
