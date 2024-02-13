namespace WebScrapping.Endpoints;

public sealed class ArticlesEndpoint
{
    public async static Task<IResult> GetNewArticles(
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(new FilterNewArticlesRequest(startDate, endDate));

        return Results.Ok(result);
    }
}