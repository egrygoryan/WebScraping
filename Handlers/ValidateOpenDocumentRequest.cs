using MediatR.Pipeline;

namespace WebScrapping.Handlers;

public sealed class ValidateOpenDocumentRequest : IRequestPreProcessor<OpenDocumentRequest>
{
    public Task Process(OpenDocumentRequest request, CancellationToken ct)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException(
                message: "Url does not well-formed",
                paramName: nameof(request.Url));
        }

        var isAllowedUriScheme = uri.Scheme switch
        {
            var scheme when
                scheme == Uri.UriSchemeHttp ||
                scheme == Uri.UriSchemeHttps => true,
            _ => false
        };

        if (!isAllowedUriScheme)
        {
            throw new ArgumentException(
                message: "Url does not represent Http(s) scheme",
                paramName: nameof(request.Url));
        }

        return Task.CompletedTask;
    }
}