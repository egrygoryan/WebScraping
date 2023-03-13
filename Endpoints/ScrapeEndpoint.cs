namespace WebScrapping.Endpoints;

public sealed class ScrapeEndpoint
{
    public static async Task<IResult> Scrape(
        [FromBody] ScrapeResource resource,
        [FromServices] IDataScrapeService dataScrapeService)
    {
        var response = await dataScrapeService.Scrape(resource.Url);

        return response.MatchFirst<IResult>(
            Results.Ok,
            x => {
                  var body = new { x.Code, x.Description };

                  return x.Type == ErrorType.NotFound 
                  ? Results.NotFound(body)
                  : Results.BadRequest(body);
                 }
            );
    }
}

public sealed record ScrapeResource(string Url);