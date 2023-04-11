namespace WebScrapping.Endpoints;

public sealed class ScrapeEndpoint
{
    public static async Task<IResult> Scrape(
        [FromBody] ScrapeResource resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.Scrape(resource.Url);

        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }

    public static async Task<IResult> ScrapeRange(
        [FromBody] ScrapeResourceRange resource,
        [FromServices] IDataScrapeService dataScrapeservice)
    {
        var response = await dataScrapeservice.ScrapeRange(resource.Url, resource.Range);
        return response.MatchFirst(
            Results.Ok,
            error => Results.Conflict(new { error.Code, error.Description }));
    }
}

public sealed record ScrapeResource(string Url);
public sealed record ScrapeResourceRange(string Url, int Range);