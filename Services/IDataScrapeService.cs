namespace WebScrapping.Services;

public interface IDataScrapeService
{
    Task<ErrorOr<ScrappedDataResponse>> ScrapeArticle(string url);
    Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog_v1(string url, int blogsRange);
    Task<ErrorOr<IEnumerable<ScrappedDataResponse>>> ScrapeBlog_v2(string url, int blogsRange);
}