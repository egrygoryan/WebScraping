namespace WebScrapping.Services;

public interface IDataScrapeService
{
    Task<ErrorOr<ScrappedDataResponse>> ScrapeArticle(string url);
    Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog(string url, int blogsRange);
}