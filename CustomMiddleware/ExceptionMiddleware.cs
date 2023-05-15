using WebScrapping.CustomExceptions;

namespace WebScrapping.CustomMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        } catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = exception switch
        {
            DocumentResponseException or ValidateUrlException => Status400BadRequest,
            HttpRequestException => Status404NotFound,
            _ => Status500InternalServerError,
        };

        await context.Response.WriteAsJsonAsync(new { exception.Message });
    }
}
