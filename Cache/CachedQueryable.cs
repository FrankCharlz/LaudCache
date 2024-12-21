using System.Collections;
using System.Linq.Expressions;

namespace LaudCache.Cache;

public class CachedQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
{
    public readonly IQueryable<T> Queryable;
    public readonly ILaudCache Cache;
    public readonly string CacheKey;
    public readonly TimeSpan Duration;

    public CachedQueryable(
        IQueryable<T> queryable,
        ILaudCache cache,
        string cacheKey,
        TimeSpan duration)
    {
        Queryable = queryable;
        Cache = cache;
        CacheKey = cacheKey;
        Duration = duration;
    }

    public Type ElementType => Queryable.ElementType;
    public Expression Expression => Queryable.Expression;
    public IQueryProvider Provider => Queryable.Provider;
    
    public IEnumerator<T> GetEnumerator() => Queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
    {
        throw new NotImplementedException("Use FirstOrDefaultAsync or ToListAsync instead");
    }
}