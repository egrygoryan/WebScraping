namespace WebScrapping.Services;

public interface IDataScrapeService
{
    ScrappedDataResponse GetScrappedData(IDocument document);
}
