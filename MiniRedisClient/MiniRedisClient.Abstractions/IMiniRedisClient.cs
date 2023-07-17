namespace MiniRedisClient.Abstractions;

public interface IMiniRedisClient
{
    Task GetAsync<TValue>(string key);
    Task AddAsync<TValue>(string key, TValue value);
    Task RemoveAsync(string key);
    Task UpdateAsync<TValue>(string key, TValue value);
}