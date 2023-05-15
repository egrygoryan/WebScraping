using System.Net;
using MediatR.Pipeline;

namespace WebScrapping.Handlers;

public sealed class VerifyOpenDocumentResponse : IRequestPostProcessor<OpenDocumentRequest, OpenDocumentResponse>
{
    private static readonly Predicate<HttpStatusCode> IsSuccessStatusCode = statusCode =>
    {
        using var message = new HttpResponseMessage(statusCode);
        return message.IsSuccessStatusCode;
    };

    public Task Process(OpenDocumentRequest request, OpenDocumentResponse response, CancellationToken ct)
    {
        var statusCode = response.Document.StatusCode;

        if (!IsSuccessStatusCode(statusCode))
        {
            throw new ApplicationException(
                message: $"Response does not indicate success status code. Status code is: '{statusCode}'");
        }

        return Task.FromResult(response);
    }
}