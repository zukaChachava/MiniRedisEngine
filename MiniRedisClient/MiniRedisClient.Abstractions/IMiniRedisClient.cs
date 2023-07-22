namespace MiniRedisClient.Abstractions;

public interface IMiniRedisClient
{
    Task<string> GetAsync(string key);
    Task<TValue> GetAsync<TValue>(string key);
    Task<string> AddAsync<TValue>(string key, TValue value);
    Task<string> RemoveAsync(string key);
    Task<string> UpdateAsync<TValue>(string key, TValue value);
}