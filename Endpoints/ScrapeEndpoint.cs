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
            error => Results.BadRequest(new { error.Code, error.Description }));
    }
}

public sealed record ScrapeResource(string Url);