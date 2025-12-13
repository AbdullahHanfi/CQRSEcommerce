using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Implementation;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Shared.Services;
/// <summary>
/// Implement Decorator Design Pattern for decorator repository using Cache Aside
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="repository"></param>
/// <param name="distributedCache"></param>
public class CachedRepository<TEntity>(Repository<TEntity> repository,
    IDistributedCache distributedCache,
    IConnectionMultiplexer redis)
    : IRepository<TEntity>
    where TEntity : class
{
    private readonly string _generalkey = typeof(TEntity).Name;
    private const int _slidingExpirationInDays = 14;
    /// <summary>
    /// Cache lower than 10k item for 14 Days with single access
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var cacheKey = $"{_generalkey}:all";
        var cacheData = await distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<IEnumerable<TEntity>>(cacheData)!;
        }
        var entites = await repository.GetAllAsync();

        if (entites.Count() < 10000)
        {
            await distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entites),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(_slidingExpirationInDays)
                });
        }
        return entites;
    }

    /// <summary>
    /// Cache item for 14 Days without single access
    /// </summary>
    /// <returns></returns>
    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        string cacheKey = $"{_generalkey}:{id}";
        var cacheData = await distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<TEntity>(cacheData);
        }

        var entity = await repository.GetByIdAsync(id);
        if (entity != null)
        {
            await distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entity),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(_slidingExpirationInDays)
                });
        }
        return entity;
    }

    /// <summary>
    /// Add item and invalidate cache for relevant effected
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task AddAsync(TEntity entity)
    {
        await repository.AddAsync(entity);

        await InvalidateCacheAsync();
    }

    /// <summary>
    /// update entity and invalidate cache for relevant effected
    /// </summary>
    /// <param name="entity"></param>
    public async void Update(TEntity entity)
    {
        repository.Update(entity);

        if (entity is BaseEntity entityWithId)
        {
            string cacheKey = $"{_generalkey}:{entityWithId.Id}";
            await distributedCache.RemoveAsync(cacheKey);
        }

        await InvalidateCacheAsync();
    }

    /// <summary>
    /// cache entity base on predicate and entity type 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        string predicateKey = GeneratePredicateKey(predicate);
        string cacheKey = $"{_generalkey}:find:{predicate}";

        var cacheData = await distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<IEnumerable<TEntity>>(cacheData)!;
        }

        var entity = await repository.FindAsync(predicate);

        if (entity != null)
        {
            await distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entity),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(_slidingExpirationInDays)
                });
        }

        return entity ?? [];
    }


    public async Task<int> CountAsync()
    {
        string cacheKey = $"{_generalkey}:count";

        var cacheData = await distributedCache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<int>(cacheData)!;
        }

        var count = await repository.CountAsync();

        await distributedCache.SetStringAsync(cacheKey,
            count.ToString(),
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(_slidingExpirationInDays)
            });

        return count;


    }
    public async void Remove(TEntity entity)
    {
        repository.Remove(entity);

        await InvalidateCacheAsync();
    }

    /// <summary>
    /// cache count base on predicate and entity type 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var predicateKey = GeneratePredicateKey(predicate);
        var cacheKey = $"{_generalkey}:count:{predicateKey}";

        var cacheData = await distributedCache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData))
        {
            return Int32.Parse(cacheData);
        }

        var count = await repository.CountAsync(predicate);

        await distributedCache.SetStringAsync(cacheKey,
            count.ToString(),
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(_slidingExpirationInDays)
            });

        return count;
    }

    /// <summary>
    /// Doesn't cache cuz it's of type IQueryable
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IQueryable<TEntity> Skip(int count)
    {
        return repository.Skip(count);
    }
    /// <summary>
    /// Doesn't cache cuz it's of type IQueryable
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IQueryable<TEntity> Take(int count)
    {
        return repository.Take(count);
    }

    /// <summary>
    /// it will invalidate all get data
    /// </summary>
    /// <returns></returns>
    private async Task InvalidateCacheAsync()
    {
        await distributedCache.RemoveAsync($"{_generalkey}:all");
        await InvalidateCacheByPatternAsync($"{_generalkey}:count*");
        await InvalidateCacheByPatternAsync($"{_generalkey}:find*");
    }
    private async Task InvalidateCacheByPatternAsync(string pattern)
    {
        var server = redis.GetServer(redis.GetEndPoints().First());
        var keys = server.Keys(pattern: pattern).ToArray();

        if (keys is not null && keys.Any())
        {
            await redis.GetDatabase().KeyDeleteAsync(keys);
        }
    }
    private string GeneratePredicateKey(Expression<Func<TEntity, bool>> predicate)
    {
        return Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(predicate.ToString())
            )
        ).Replace("/", "_").Replace("+", "-");
    }

}
