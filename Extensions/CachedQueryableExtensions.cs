using LaudCache.Models;
using LaudCache.Src;
using Microsoft.EntityFrameworkCore;

namespace LaudCache.Extensions;

public static class CachedQueryableExtensions
{
    public static CachedQueryable<T> WithCache<T>(
        this IQueryable<T> query,
        ILaudCache cacheHelper,
        string cacheKey,
        TimeSpan duration
    )
    {
        return new CachedQueryable<T>(query, cacheHelper, cacheKey, duration);
    }

    public static async Task<T?> FirstOrDefaultAsync<T>(
        this CachedQueryable<T> query,
        CancellationToken cancellationToken = default
    )
    {
        var cachedResult = await query.Cache.GetOrAddAsync(
            query.CacheKey,
            async () => await query.Queryable.FirstOrDefaultAsync(cancellationToken),
            query.Duration
        );


        return cachedResult;
    }

    public static async Task<List<T>> ToListAsync<T>(
        this CachedQueryable<T> query,
        CancellationToken cancellationToken = default
    )
    {
        var cachedResult = await query.Cache.GetOrAddAsync(
            query.CacheKey,
            async () => await query.Queryable.ToListAsync(cancellationToken),
            query.Duration
        );


        return cachedResult;
    }


    public static async Task<PagedResult<T>> ToListPaginatedAsync<T>(
        this CachedQueryable<T> query,
        int pageNo,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var data = query.Cache.Get<PagedResult<T>>(query.CacheKey);
        if (data != null) return data;

        data = new PagedResult<T>
        (
            count: await query.CountAsync(cancellationToken),
            items: await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
        );

        query.Cache.Add(query.CacheKey, data, query.Duration, false);

        return data;
    }
    
    
    

}