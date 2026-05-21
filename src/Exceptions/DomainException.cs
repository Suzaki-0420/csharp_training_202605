namespace WebApp_Sample.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message)
    : base() { }
    public DomainException(string message, Exception innerException)
    : base(message, innerException) { }
}