namespace WebScrapping.Handlers;

public sealed class OpenDocumentHandler : IRequestHandler<OpenDocumentRequest, OpenDocumentResponse>
{
    private readonly IBrowsingContext _browser;

    public OpenDocumentHandler(IBrowsingContext browser) => _browser = browser;

    public async Task<OpenDocumentResponse> Handle(OpenDocumentRequest request, CancellationToken ct)
    {
        var document = await _browser.OpenAsync(request.Url, ct);

        return new OpenDocumentResponse(document);
    }
}