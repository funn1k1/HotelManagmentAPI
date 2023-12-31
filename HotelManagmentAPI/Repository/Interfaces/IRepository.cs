﻿using System.Linq.Expressions;

namespace HotelManagment_API.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? includeProperties = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool isTracked = true,
            int pageNumber = 0,
            int pageSize = 0
        );

        Task<T?> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? includeProperties = null,
            bool isTracked = true
        );

        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task SaveAsync();
    }
}
