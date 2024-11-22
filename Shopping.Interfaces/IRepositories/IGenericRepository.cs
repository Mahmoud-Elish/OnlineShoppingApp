using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<int> AddAsync(T entity);
    Task<int> DeleteAsync(int id);
    IQueryable<T> GetQueryable();
    Task<T> GetByIdAsync(int id);
    Task<int> UpdateAsync(T entity);
}
