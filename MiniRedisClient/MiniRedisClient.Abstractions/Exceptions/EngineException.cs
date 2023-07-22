namespace MiniRedisClient.Abstractions.Exceptions;

public class EngineException : MiniRedisException
{
    public EngineException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}