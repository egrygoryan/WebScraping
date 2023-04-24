using MediatR;
using WebScrapping.Handlers.Requests;
using WebScrapping.Handlers.Responses;

namespace WebScrapping.Handlers.Option_Two;

public sealed class OpenDocumentHandler : IRequestHandler<OpenDocumentRequest_v2, OpenDocumentResponse>
{
    private readonly IBrowsingContext _browser;

    public OpenDocumentHandler(IBrowsingContext browser) => _browser = browser;

    public async Task<OpenDocumentResponse> Handle(OpenDocumentRequest_v2 request, CancellationToken ct)
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

        var document = await _browser.OpenAsync(request.Url, ct);

        using var response = new HttpResponseMessage(document.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(
                message: $"Response does not indicate success status code. Status code is: '{document.StatusCode}'");
        }

        return new OpenDocumentResponse(document);
    }
}