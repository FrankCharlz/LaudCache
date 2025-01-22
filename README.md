# LaudCache

## A Redis caching layer designed for seamless EntityFrameworkCore integration and more

### Usage:

> - Setup (Startup.cs)

```

using LaudCache.Extensions;

namespace Example;

public class Startup
{


    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddRedis(
            address: "localhost:6379",
            defaultDatabase: 0,
            instanceName: "my-project-name"
        );

        // ... other codes ...

    }

}
```

> - Dependency injection (WorkerService.cs)

```

namespace Example.Utils;

public class WorkerService
{
    private readonly LaudCache.Cache.LaudCache _laudCache;
    private readonly ExDbContextSqlServer _dbContext;

    public WorkerService(IDbContextFactory<MembershipContextSqlServer> contextFactory,
        LaudCache.Cache.LaudCache laudCache)
    {
        _laudCache = laudCache;
        _context = contextFactory.CreateDbContext();
    }
    
    // ...
    
}

```

> - Actual usage (WorkerService.cs)

```

namespace Example.Utils;

public class WorkerService
{
    private readonly LaudCache.Cache.LaudCache _laudCache;
    private readonly ExDbContextSqlServer _dbContext;

    
    // ...
    
    // Example 1:
    
    [HttpGet]
    [Authorize]
    public IActionResult Notifications()
    {
        var username = User.Identity?.Name!;
        
        var notifications = _dbContext.Notifications
            .Where(n => n.Username == username)
            .WithCache(_laudCache, "my-cache-key", TimeSpan.FromHours(4))
            .ToListAsync();
            
        return new Ok(notifications);
    }
    
    
    // Example 2:
     
    [HttpGet]
    public async Task<IActionResult> Card()
    {
        var username = User.Identity?.Name!;
        
        var cacheKey = "my-cache-key-2";
        
        var data0 = _laudCache.Get<object>(cacheKey);
        if (data0 != null) return Ok(data0);

        var card = await _membershipContext
            .Cards
            .Where(m => m.CardNo == username)
            .FirstOrDefaultAsync();

        if (card == null) return NotFound();

        _distributedCacheHelper.Add(cacheKey, card, TimeSpan.FromHours(12);
        
        
        return Ok(card);
    }
    
}

```