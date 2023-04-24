using AngleSharp.Dom;
using MediatR;
using WebScrapping.Handlers.Requests;

namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IMediator _mediator;
    private readonly IBrowsingContext _browser;

    public DataScrapeService(IMediator mediator, IBrowsingContext browser)
    {
        _mediator = mediator;
        _browser = browser;
    }

    public async Task<ErrorOr<ScrappedDataResponse>> ScrapeArticle(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return Error.Validation(
                code: "Uri.Validation",
                description: "Url does not well-formed");
        }
        
        if (uri.Scheme is not ("http" or "https"))
        {
            return Error.Validation(
                code: "Uri.Validation",
                description: "Url does not represent Http(s) scheme");
        }
        var document = await _browser.OpenAsync(url);

        var response = new HttpResponseMessage(document.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            return Error.Failure(
                code: "Document.Load.Failure",
                description: $"Response does not indicate success status code. Status code is: '{document.StatusCode}'");
        }

        var title = document.Title;
        var origin = document.Origin;

        var descriptionSelector = document.QuerySelector("meta[name=description]")
                                ?? document.QuerySelector("meta[property='og:description']");
        var description = descriptionSelector?.GetAttribute("content");

        var author = document.QuerySelector("meta[name=author]")?.GetAttribute("content")
                   ?? document.QuerySelector(".author-name > a")?.TextContent;

        string scrappedDate = document.QuerySelector("meta[property='article:published_time']")?.GetAttribute("content");
        string formattedDate = string.Empty;
        if(DateTime.TryParse(scrappedDate, out var format))
        {
            formattedDate = format.ToString("hh:mm:ss dd/MM/yyyy");
        };

        return new ScrappedDataResponse(origin, author, title, description, formattedDate);
    }

    public async Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog_v1(string url, int blogsRange)
    {
        var response = await _mediator.Send(new OpenDocumentRequest_v1(url));
        var origin = response.Document.Location.Origin;

        var articles = response.Document
            .QuerySelectorAll("article")
            .Take(blogsRange)
            .Select(article => article
                .QuerySelector("a.post-card-content-link")?
                .GetAttribute("href") ?? "")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => new Uri(new (origin), x))
            .Select(x => ScrapeArticle(x.ToString()))
            .ToList();

        var awaiter = await Task.WhenAll(articles);

        var scrapedArticles = awaiter
            .Where(x => !x.IsError)
            .Select(x => x.Value)
            .ToList();

        return scrapedArticles;
    }
    
    public async Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog_v2(string url, int blogsRange)
    {
        var response = await _mediator.Send(new OpenDocumentRequest_v2(url));
        var origin = response.Document.Location.Origin;

        var articles = response.Document
            .QuerySelectorAll("article")
            .Take(blogsRange)
            .Select(article => article
                .QuerySelector("a.post-card-content-link")?
                .GetAttribute("href") ?? "")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => new Uri(new (origin), x))
            .Select(x => ScrapeArticle(x.ToString()))
            .ToList();

        var awaiter = await Task.WhenAll(articles);

        var scrapedArticles = awaiter
            .Where(x => !x.IsError)
            .Select(x => x.Value)
            .ToList();

        return scrapedArticles;
    }
}