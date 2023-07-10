namespace WebScrapping.Handlers;

public sealed class OpenDocumentHandler : IRequestHandler<OpenDocumentRequest, ErrorOr<OpenDocumentResponse>>
{
    private readonly IBrowsingContext _browser;

    public OpenDocumentHandler(IBrowsingContext browser) => _browser = browser;

    public async Task<ErrorOr<OpenDocumentResponse>> Handle(OpenDocumentRequest request, CancellationToken ct)
    {
        var document = await _browser.OpenAsync(request.Url, ct);

        return new OpenDocumentResponse(document);
    }
}