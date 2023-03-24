namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IBrowsingContext _browser;

    public DataScrapeService(IBrowsingContext browser) => _browser = browser;

    public async Task<ErrorOr<ScrappedDataResponse>> Scrape(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return Error.Validation(
                code: "Uri.Validation",
                description: "Url does not well-formed");
        }
        
        var document = await _browser.OpenAsync(url);
        
        var response = new HttpResponseMessage(document.StatusCode);
        
        if(!response.IsSuccessStatusCode)
        {
            return Error.Failure(
                code: "Document.Load.Failure",
                description: $"Response does not indicate success status code. Status code is: '{document.StatusCode}'");
        }
        
        var title = document.Title;
        var origin = document.Origin;
        var description = document.QuerySelector("meta[name=description]")?.GetAttribute("content");
        var author = document.QuerySelector("meta[name=author]")?.GetAttribute("content");

        return new ScrappedDataResponse(origin, author, title, description);
    }
}
