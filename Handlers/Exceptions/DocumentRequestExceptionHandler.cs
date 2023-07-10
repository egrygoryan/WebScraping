using MediatR.Pipeline;

namespace WebScrapping.Handlers.Exceptions;

public sealed class DocumentRequestExceptionHandler : IRequestExceptionHandler<OpenDocumentRequest, ErrorOr<OpenDocumentResponse>, Exception>
{
    public Task Handle(OpenDocumentRequest request, Exception exception, RequestExceptionHandlerState<ErrorOr<OpenDocumentResponse>> state, CancellationToken cancellationToken)
    {
        var code = exception switch
        {
            ApplicationException => "Document.Failure",
            ArgumentException => "Url.Failure",
            _ => "General.Failure"
        };

        var error = Error.Failure(
            code: code,
            description: exception.Message);

        state.SetHandled(error);

        return Task.CompletedTask;
    }
}