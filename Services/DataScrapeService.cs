namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IMediator _mediator;

    public DataScrapeService(IMediator mediator) => _mediator = mediator;

    public async Task<ErrorOr<ScrappedDataResponse>> ScrapeArticle(string url)
    {
        var response = await _mediator.Send(new OpenDocumentRequest(url));
        if (response.IsError)
        {
            return ErrorOr<ScrappedDataResponse>
                .From(response.Errors);
        }

        var document = response.Value.Document;

        var title = document.Title;
        var origin = document.Location.Origin;

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

    public async Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog(string url, int blogsRange)
    {
        var response = await _mediator.Send(new OpenDocumentRequest(url));
        if (response.IsError)
        {
            return ErrorOr<IEnumerable<ScrappedDataResponse>>
                .From(response.Errors);
        }
        var document = response.Value.Document;

        var origin = document.Location.Origin;

        var articles = document
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