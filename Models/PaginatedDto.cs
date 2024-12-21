namespace LaudCache.Models;



public record PaginatedDto<T>(int count, List<T> items);