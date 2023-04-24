using MediatR.Pipeline;
using WebScrapping.Handlers.Requests;

namespace WebScrapping.Handlers.Option_One;

public sealed class ValidateOpenDocumentRequest : IRequestPreProcessor<OpenDocumentRequest_v1>
{
    public Task Process(OpenDocumentRequest_v1 request, CancellationToken ct)
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
        
        //
        // Alternative option
        //
        // var isAllowedUriScheme = uri.Scheme != Uri.UriSchemeHttp || uri.Scheme != Uri.UriSchemeHttps;
        //
        
        if (!isAllowedUriScheme)
        {
            throw new ArgumentException(
                message: "Url does not represent Http(s) scheme",
                paramName: nameof(request.Url));
        }

        return Task.CompletedTask;
    }
}