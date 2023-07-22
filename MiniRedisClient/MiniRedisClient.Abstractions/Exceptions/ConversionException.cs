namespace MiniRedisClient.Abstractions.Exceptions;

public class ConversionException : MiniRedisException
{
    public ConversionException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }
}