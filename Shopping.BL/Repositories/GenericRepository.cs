using Microsoft.EntityFrameworkCore;
using Shopping.Interfaces;
using Shopping.DAL;
using System.Linq.Expressions;

namespace Shopping.BL;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public IQueryable<T> GetQueryable()
    { 
        return _dbSet.AsNoTracking(); 
    }
    public IQueryable<T> GetQueryable(int pageNumber = 1,int pageSize = 30)
    {
        return _dbSet.AsNoTracking().Skip(pageNumber * pageSize).Take(pageSize);
    }
    public IQueryable<T> GetQueryable(int id)
    {
        return _dbSet.Where(e => EF.Property<int>(e, "Id") == id);
    }
    public async Task<int> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }
    public async Task<int> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync();
    }
}
