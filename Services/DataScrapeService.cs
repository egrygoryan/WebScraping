namespace WebScrapping.Services;

public sealed class DataScrapeService : IDataScrapeService
{
    public ScrappedDataResponse GetScrappedData(IDocument document)
    {
        var title = document.Title;
        var origin = document.Origin;
        var description = document.QuerySelector("meta[property*=description]")?.GetAttribute("content");

        return new ScrappedDataResponse(origin, title, description);
    }
}
