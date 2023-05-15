namespace WebScrapping.CustomExceptions;

public sealed class DocumentResponseException : Exception
{
    public DocumentResponseException() : base() { }
    public DocumentResponseException(string message) : base(message) { }
    public DocumentResponseException(string message, Exception innerException) 
        : base(message, innerException) { }
}