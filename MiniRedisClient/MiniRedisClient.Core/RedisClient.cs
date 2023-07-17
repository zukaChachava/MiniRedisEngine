using System.Net.Sockets;
using System.Text;
using MiniRedisClient.Abstractions;

namespace MiniRedisClient.Core;

/// <summary>
/// Mini Redis Client Implementation. Object is thread safe, so that it should be used as singleton.
/// </summary>
public class RedisClient : IMiniRedisClient
{
    private readonly TcpClient _tcpClient;

    public RedisClient(TcpClient tcpClient)
    {
        _tcpClient = tcpClient;
    }

    public RedisClient(string hostname, int port) : this(new TcpClient(hostname, port)){}

    public RedisClient(): this("127.0.0.1", 9009){}


    public Task GetAsync<TValue>(string key)
    {
        byte[] bytes = GenerateBytes(GenerateMessage(Method.Get, key, string.Empty));
        using NetworkStream networkStream = _tcpClient.GetStream();
        return networkStream.WriteAsync(bytes, 0, bytes.Length);
    }

    public Task AddAsync<TValue>(string key, TValue value)
    {
        byte[] bytes = GenerateBytes(GenerateMessage(Method.Add, key, value?.ToString() ?? string.Empty));
        using NetworkStream networkStream = _tcpClient.GetStream();
        return networkStream.WriteAsync(bytes, 0, bytes.Length);
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync<TValue>(string key, TValue value)
    {
        throw new NotImplementedException();
    }

    private string GenerateMessage(Method method, string key, string value)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder
            .Append(method)
            .Append('\0')
            .Append(key)
            .Append('\0')
            .Append(value)
            .Append('\0');

        return stringBuilder.ToString();
    }

    private byte[] GenerateBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }
}