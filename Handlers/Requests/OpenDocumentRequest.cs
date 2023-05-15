namespace WebScrapping.Handlers.Requests;

public record OpenDocumentRequest(string Url) : IRequest<OpenDocumentResponse>;