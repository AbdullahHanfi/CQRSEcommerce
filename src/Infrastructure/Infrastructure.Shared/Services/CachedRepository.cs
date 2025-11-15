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
/// Implements Decorator Pattern for repository using Cache-Aside pattern
/// </summary>
public class CachedRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly IRepository<TEntity> _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _redis;
    private readonly string _generalKey;
    private readonly TimeSpan _slidingExpiration;
    private readonly TimeSpan _absoluteExpiration;

    public CachedRepository(
        Repository<TEntity> decorated,
        IDistributedCache distributedCache,
        IConnectionMultiplexer redis)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _redis = redis;
        _generalKey = typeof(TEntity).Name;
        _slidingExpiration = TimeSpan.FromDays(1);
        _absoluteExpiration = TimeSpan.FromDays(7);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var cacheKey = $"{_generalKey}:all";
        var cacheData = await _distributedCache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<IEnumerable<TEntity>>(cacheData) ?? [];
        }

        var entities = await _decorated.GetAllAsync();

        if (entities.Any())
        {
            await _distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entities),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _slidingExpiration,
                    AbsoluteExpirationRelativeToNow = _absoluteExpiration
                });
        }
        
        return entities;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"{_generalKey}:{id}";
        var cacheData = await _distributedCache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<TEntity>(cacheData);
        }

        var entity = await _decorated.GetByIdAsync(id);
        
        if (entity != null)
        {
            await _distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entity),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _slidingExpiration,
                    AbsoluteExpirationRelativeToNow = _absoluteExpiration
                });
        }
        
        return entity;
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var predicateKey = GeneratePredicateKey(predicate);
        var cacheKey = $"{_generalKey}:find:{predicateKey}";

        var cacheData = await _distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<IEnumerable<TEntity>>(cacheData) ?? [];
        }

        var entities = await _decorated.FindAsync(predicate);
        var entityList = entities.ToList();

        if (entityList.Any())
        {
            await _distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(entityList),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _slidingExpiration,
                    AbsoluteExpirationRelativeToNow = _absoluteExpiration
                });
        }

        return entityList;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _decorated.AddAsync(entity);
        await InvalidateCacheAsync();
    }

    public void Update(TEntity entity)
    {
        _decorated.Update(entity);
        _ = InvalidateCacheAsync();
    }

    public void Remove(TEntity entity)
    {
        _decorated.Remove(entity);
        _ = InvalidateCacheAsync();
    }

    public async Task<int> CountAsync()
    {
        var cacheKey = $"{_generalKey}:count";
        var cacheData = await _distributedCache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData) && int.TryParse(cacheData, out var count))
        {
            return count;
        }

        count = await _decorated.CountAsync();
        
        await _distributedCache.SetStringAsync(cacheKey,
            count.ToString(),
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = _slidingExpiration,
                AbsoluteExpirationRelativeToNow = _absoluteExpiration
            });

        return count;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var predicateKey = GeneratePredicateKey(predicate);
        var cacheKey = $"{_generalKey}:count:{predicateKey}";
        var cacheData = await _distributedCache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData) && int.TryParse(cacheData, out var count))
        {
            return count;
        }

        count = await _decorated.CountAsync(predicate);
        
        await _distributedCache.SetStringAsync(cacheKey,
            count.ToString(),
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = _slidingExpiration,
                AbsoluteExpirationRelativeToNow = _absoluteExpiration
            });

        return count;
    }

    public IQueryable<TEntity> Skip(int count) => _decorated.Skip(count);
    public IQueryable<TEntity> Take(int count) => _decorated.Take(count);

    private async Task InvalidateCacheAsync()
    {
        try
        {
            await _distributedCache.RemoveAsync($"{_generalKey}:all");
            await InvalidateCacheByPatternAsync($"{_generalKey}:count*");
            await InvalidateCacheByPatternAsync($"{_generalKey}:find*");
        }
        catch (Exception ex)
        {
            // Log cache invalidation errors but don't fail the operation
            Console.WriteLine($"Cache invalidation error: {ex.Message}");
        }
    }

    private async Task InvalidateCacheByPatternAsync(string pattern)
    {
        try
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"*{pattern}*").ToArray();
            
            if (keys.Any())
            {
                var db = _redis.GetDatabase();
                await Task.WhenAll(keys.Select(key => db.KeyDeleteAsync(key)));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pattern cache invalidation error: {ex.Message}");
        }
    }

    private static string GeneratePredicateKey(Expression<Func<TEntity, bool>> predicate)
    {
        var expressionString = predicate.ToString();
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(expressionString));
        return Convert.ToBase64String(hash)
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=", "");
    }
}