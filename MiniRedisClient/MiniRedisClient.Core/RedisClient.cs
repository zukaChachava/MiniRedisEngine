using System.Net.Sockets;
using System.Text;
using MiniRedisClient.Abstractions;
using MiniRedisClient.Abstractions.Exceptions;

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

    public async Task<string> GetAsync(string key)
    {
        var response = await ExecuteRequestAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Get, key, string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
            
            byte[] responseBytes = new byte[256];
            int _ = await networkStream.ReadAsync(responseBytes);
            return responseBytes;
        });
        
        return ProcessResponse(response);
    }

    public async Task<TValue> GetAsync<TValue>(string key)
    {
        string value = await GetAsync(key);
        return (TValue)(Convert.ChangeType(value, typeof(TValue)) ?? throw new ConversionException($"Can not convert string to {typeof(TValue)}"));
    }

    public async Task<string> AddAsync<TValue>(string key, TValue value)
    {
        var response = await ExecuteRequestAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Add, key, value?.ToString() ?? string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();

            byte[] responseBytes = new byte[256];
            int _ = await networkStream.ReadAsync(responseBytes);
            return responseBytes;
        });

        return ProcessResponse(response);
    }

    public async Task<string> RemoveAsync(string key)
    {
        var response = await ExecuteRequestAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Remove, key, string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
            
            byte[] responseBytes = new byte[256];
            int _ = await networkStream.ReadAsync(responseBytes);
            return responseBytes;
        });

        return ProcessResponse(response);
    }

    public async Task<string> UpdateAsync<TValue>(string key, TValue value)
    {
        var response = await ExecuteRequestAsync(async (networkStream) =>
        {
            byte[] bytes = GenerateBytes(GenerateMessage(Method.Update, key, value?.ToString() ?? string.Empty));
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
            await networkStream.FlushAsync();
            
            byte[] responseBytes = new byte[256];
            int _ = await networkStream.ReadAsync(responseBytes);
            return responseBytes;
        });
        
        return ProcessResponse(response);
    }

    private async Task<byte[]> ExecuteRequestAsync(Func<NetworkStream, Task<byte[]>> operations)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_host, _port);
        await using NetworkStream networkStream = client.GetStream();
        return await operations.Invoke(networkStream);
    }

    private string GenerateMessage(Method method, string key, string value)
    {
        return new StringBuilder()
            .Append((char)method)
            .Append('\0')
            .Append(key)
            .Append('\0')
            .Append(value)
            .Append('\0').ToString();
    }

    private byte[] GenerateBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }

    private string ProcessResponse(byte[] message)
    {
        (var responseType, string response) = SplitResponseMessage(message);

        switch (responseType)
        {
            case ResponseType.Error:
                throw new EngineException(response);
            case ResponseType.NotFound:
                throw new NotFoundException(response);
            default:
                break;
        }
        
        return response;
    }

    private (ResponseType responseType, string message) SplitResponseMessage(byte[] message)
    {
        byte[,] messageParts = new byte[2, 256];
        byte state = 0;
        short index = 0;
        
        foreach (var data in message)
        {
            if(state == 2)
                break;
            
            if (data == 0)
            {
                index = 0;
                state++;
                continue;
            }

            messageParts[state, index++] = data;
        }

        return ((ResponseType)messageParts[0,0], 
            Encoding.UTF8.GetString(
                Enumerable.Range(0, messageParts.GetLength(1))
                    .Select(i => messageParts[state - 1, i]).ToArray()));
    }
}