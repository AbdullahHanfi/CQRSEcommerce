using Domain.Repositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Implementation;

public class Repository<TEntity>(AuthDbContext context) 
    : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _entity= context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
    }

    public async Task<int> CountAsync()
    {
        return await _entity.CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entity.CountAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entity.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entity.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _entity.FindAsync(id);
    }

    public void Remove(TEntity entity)
    {
        _entity.Remove(entity);
    }
    
    public IQueryable<TEntity> Skip(int count)
    {
        return _entity.Skip(count);
    }

    public IQueryable<TEntity> Take(int count)
    {
        return _entity.Take(count);
    }

    public void Update(TEntity entity)
    {
        _entity.Update(entity);
    }
}
