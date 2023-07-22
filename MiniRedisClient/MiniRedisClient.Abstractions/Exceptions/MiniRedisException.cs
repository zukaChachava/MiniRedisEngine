namespace MiniRedisClient.Abstractions.Exceptions;

public class MiniRedisException : Exception
{
    public MiniRedisException(string message, Exception? innerException = null):base(message, innerException){}
}