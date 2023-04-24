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

    public static async Task<IResult> ScrapeBlog_v1(
        [FromBody] ScrapeResourceRange resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.ScrapeBlog_v1(resource.Url, resource.Range);
        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }
    
    public static async Task<IResult> ScrapeBlog_v2(
        [FromBody] ScrapeResourceRange resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.ScrapeBlog_v2(resource.Url, resource.Range);
        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }
}

public sealed record ScrapeResource(string Url);
public sealed record ScrapeResourceRange(string Url, int Range);