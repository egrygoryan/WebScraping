using ErrorOr;

namespace WebScrapping.Services;

public interface IDataScrapeService
{
    Task<ErrorOr<ScrappedDataResponse>> Scrape(string url);
}
