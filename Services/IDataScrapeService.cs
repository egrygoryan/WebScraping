namespace WebScrapping.Services;

public interface IDataScrapeService
{
    Task<ErrorOr<ScrappedDataResponse>> Scrape(string url);
    Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeRange(string url, int blogsRange);
}