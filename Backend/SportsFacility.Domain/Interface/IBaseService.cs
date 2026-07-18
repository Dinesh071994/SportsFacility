using SportsFacility.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsFacility.Domain.Interface
{
    public interface IBaseService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<bool> DeleteAsync(int id);
    }
}
