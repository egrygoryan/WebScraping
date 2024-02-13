namespace WebScrapping.Handlers.Requests;

public sealed class FilterNewArticlesRequest : IRequest<IEnumerable<FilterNewArticlesResponse>>
{
    private readonly TimeSpan _endOffset = new(23, 59, 59);
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public FilterNewArticlesRequest(DateTime? startDate, DateTime? endDate)
    {
        StartDate = startDate ?? default;
        EndDate = (endDate ?? DateTime.UtcNow)
            .Date
            .Add(_endOffset);
    }
}