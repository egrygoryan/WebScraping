namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IMediator _mediator;

    public DataScrapeService(IMediator mediator) => _mediator = mediator;

    public async Task<ErrorOr<ScrappedDataResponse>> ScrapeArticle(string url)
    {
        var response = await _mediator.Send(new OpenDocumentRequest(url));

        var title = response.Document.Title;
        var origin = response.Document.Location.Origin;

        var descriptionSelector = response.Document.QuerySelector("meta[name=description]")
                                ?? response.Document.QuerySelector("meta[property='og:description']");
        var description = descriptionSelector?.GetAttribute("content");

        var author = response.Document.QuerySelector("meta[name=author]")?.GetAttribute("content")
                   ?? response.Document.QuerySelector(".author-name > a")?.TextContent;

        string scrappedDate = response.Document.QuerySelector("meta[property='article:published_time']")?.GetAttribute("content");
        string formattedDate = string.Empty;
        if(DateTime.TryParse(scrappedDate, out var format))
        {
            formattedDate = format.ToString("hh:mm:ss dd/MM/yyyy");
        };

        return new ScrappedDataResponse(origin, author, title, description, formattedDate);
    }

    public async Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog(string url, int blogsRange)
    {
        var response = await _mediator.Send(new OpenDocumentRequest(url));
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