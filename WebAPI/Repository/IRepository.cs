﻿using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        void SetDbContext(DbContext dbContext);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
