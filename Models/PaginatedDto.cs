namespace LaudCache.Models;



public record PagedResult<T>(int count, List<T> items);