namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    private readonly IBrowsingContext _browser;

    public DataScrapeService(IBrowsingContext browser) => _browser = browser;
    
    public async Task<ErrorOr<ScrappedDataResponse>> Scrape(string url)
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

    public async Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeRange(string url, int blogsRange)
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

        var blogs = document.QuerySelectorAll("article").Take(blogsRange);

        StringBuilder sb = new();

        var links = blogs.Aggregate(new List<string>(), (blogsList, blog) =>
        {
            var relative = blog.QuerySelector("a.post-card-content-link")?.GetAttribute("href");
            sb.Clear();
            sb.Append(url.TrimEnd('/')).Append(relative);
            blogsList.Add(sb.ToString());

            return blogsList;
        });

 
        var blogsData = await Task.WhenAll(links.Select(async blog => (await Scrape(blog)).Value));

        return blogsData;
    }
}