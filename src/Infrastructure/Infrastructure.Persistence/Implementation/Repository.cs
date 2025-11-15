using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Implementation;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly AuthDbContext _context;
    protected readonly DbSet<TEntity> _entity;

    public Repository(AuthDbContext context)
    {
        _context = context;
        _entity = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _entity.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entity.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entity.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _entity.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        _entity.Remove(entity);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _entity.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entity.CountAsync(predicate);
    }

    public virtual IQueryable<TEntity> Skip(int count)
    {
        return _entity.Skip(count);
    }

    public virtual IQueryable<TEntity> Take(int count)
    {
        return _entity.Take(count);
    }
}