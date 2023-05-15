namespace WebScrapping.CustomExceptions;

public sealed class ValidateUrlException : Exception
{
    public ValidateUrlException() { }

    public ValidateUrlException(string message) : base(message) { }
    public ValidateUrlException(string message, Exception innerException) 
        : base(message, innerException) { }
}