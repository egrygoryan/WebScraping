namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IBrowsingContext _browser;

    public DataScrapeService(IBrowsingContext browser) => _browser = browser;

    public async Task<ErrorOr<ScrappedDataResponse>> Scrape(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return Error.Validation();
        }

        var document = await _browser.OpenAsync(url);
        if(document is null)
        {
            return Error.NotFound();
        }

        var title = document.Title;
        var origin = document.Origin;
        var description = document.QuerySelector("meta[name=description]")?.GetAttribute("content");
        var author = document.QuerySelector("meta[name=author]")?.GetAttribute("content");

        return new ScrappedDataResponse(origin, author, title, description);
    }
}
