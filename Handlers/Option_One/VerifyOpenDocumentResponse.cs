using System.Net;
using MediatR.Pipeline;
using WebScrapping.Handlers.Requests;
using WebScrapping.Handlers.Responses;

namespace WebScrapping.Handlers.Option_One;

public sealed class VerifyOpenDocumentResponse : IRequestPostProcessor<OpenDocumentRequest_v1, OpenDocumentResponse>
{
    private static readonly Predicate<HttpStatusCode> IsSuccessStatusCode = statusCode =>
    {
        using var message = new HttpResponseMessage(statusCode);
        return message.IsSuccessStatusCode;
    };

    public Task Process(OpenDocumentRequest_v1 request, OpenDocumentResponse response, CancellationToken ct)
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