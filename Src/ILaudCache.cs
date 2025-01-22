namespace LaudCache.Src;

public interface ILaudCache
{
    public T? Get<T>(string key);
    public void Add<T>(string key, T value, TimeSpan duration, bool isSliding = true);
    public T GetOrAdd<T>(string key, Func<Task<T>> valueFactory, TimeSpan duration, bool isSliding = true);
    public  Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan duration);
    public  Task RemoveKeyAsync(string key);
}