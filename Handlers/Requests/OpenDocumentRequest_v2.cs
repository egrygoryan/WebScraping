using MediatR;
using WebScrapping.Handlers.Responses;

namespace WebScrapping.Handlers.Requests;

public sealed record OpenDocumentRequest_v2(string Url) : IRequest<OpenDocumentResponse>;