using MediatR;
using WebScrapping.Handlers.Requests;
using WebScrapping.Handlers.Responses;

namespace WebScrapping.Handlers.Option_One;

public sealed class OpenDocumentHandler : IRequestHandler<OpenDocumentRequest_v1, OpenDocumentResponse>
{
    private readonly IBrowsingContext _browser;

    public OpenDocumentHandler(IBrowsingContext browser) => _browser = browser;

    public async Task<OpenDocumentResponse> Handle(OpenDocumentRequest_v1 request, CancellationToken ct)
    {
        var document = await _browser.OpenAsync(request.Url, ct);

        return new OpenDocumentResponse(document);
    }
    
}