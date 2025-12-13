namespace Domain.Repositories;

using System.Linq.Expressions;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> Skip(int count);
    IQueryable<TEntity> Take(int count);

    void Update(TEntity entity);
    void Remove(TEntity entity);

}