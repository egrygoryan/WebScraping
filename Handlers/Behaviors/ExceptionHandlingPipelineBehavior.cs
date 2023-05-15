using WebScrapping.CustomExceptions;

namespace WebScrapping.Handlers.Behaviors
{
    public class ExceptionHandlingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<OpenDocumentResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            } catch (Exception ex)
            {
                var exception = ex switch
                {
                    ApplicationException => new DocumentResponseException(ex.Message),
                    ArgumentException => new ValidateUrlException(ex.Message),
                    _ => new Exception(ex.Message)
                };

                throw exception;
            }
        }
    }
}
