﻿using System.Net.Sockets;
using System.Text;
using MiniRedisClient.Abstractions;

namespace MiniRedisClient.Core;

/// <summary>
/// Mini Redis Client Implementation. Object is thread safe, so that it should be used as singleton.
/// </summary>
public class RedisClient : IMiniRedisClient
{
    private readonly string _host;
    private readonly int _port;

    public RedisClient(string hostname, int port)
    {
        _host = hostname;
        _port = port;
    }

    public RedisClient(): this("127.0.0.1", 9009){}

    public async Task GetAsync<TValue>(string key)
    {
        await ManageConnectionAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Get, key, string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
        });
    }

    public async Task AddAsync<TValue>(string key, TValue value)
    {
        await ManageConnectionAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Add, key, value?.ToString() ?? string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
        });
    }

    public async Task RemoveAsync(string key)
    {
        await ManageConnectionAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Remove, key, string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
        });
    }

    public async Task UpdateAsync<TValue>(string key, TValue value)
    {
        await ManageConnectionAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Remove, key, value?.ToString() ?? string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
        });
    }

    private async Task ManageConnectionAsync(Func<NetworkStream, Task> operations)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_host, _port);
        await using NetworkStream networkStream = client.GetStream();
        await operations.Invoke(networkStream);
    }

    private string GenerateMessage(Method method, string key, string value)
    {
        return new StringBuilder()
            .Append((byte)method)
            .Append('\0')
            .Append(key)
            .Append('\0')
            .Append(value)
            .Append('\0').ToString();
    }

    private byte[] GenerateBytes(string message)
    {
        byte[] encodedData =  Encoding.UTF8.GetBytes(message);
        encodedData[0] = (byte)(encodedData[0] - 48);
        return encodedData;
    }
}