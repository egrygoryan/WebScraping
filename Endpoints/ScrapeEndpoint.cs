namespace WebScrapping.Endpoints;

public sealed class ScrapeEndpoint
{
    public static async Task<IResult> ScrapeArticle(
        [FromBody] ScrapeResource resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.ScrapeArticle(resource.Url);

        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }

    public static async Task<IResult> ScrapeBlog(
        [FromBody] ScrapeResourceRange resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.ScrapeBlog(resource.Url, resource.Range);
        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }
}