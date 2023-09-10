﻿using System.Linq.Expressions;

namespace HotelManagmentAPI.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool isTracked = true);

        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null);

        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task SaveAsync();
    }
}
