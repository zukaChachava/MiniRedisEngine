namespace MiniRedisClient.Abstractions.Exceptions;

public class NotFoundException : MiniRedisException
{
    public NotFoundException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}