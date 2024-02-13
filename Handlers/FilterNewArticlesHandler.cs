namespace WebScrapping.Handlers;

public sealed class FilterNewArticlesHandler(
    IArticleRepository articleRepo) : IRequestHandler<FilterNewArticlesRequest, IEnumerable<FilterNewArticlesResponse>>
{
    public async Task<IEnumerable<FilterNewArticlesResponse>> Handle(FilterNewArticlesRequest request, CancellationToken cancellationToken)
    {
        var filteredArticles = articleRepo.GetNewArticles(request);

        var result = filteredArticles.Select(x =>
            new FilterNewArticlesResponse(
                Origin: x.Origin,
                Author: x.Author,
                Title: x.Title,
                Description: x.Description,
                PublishedDate: x.PublishedDate.ToString("hh:mm:ss dd/MM/yyyy"),
                Originality: x.Originality.ToString()))
            .ToList();

        return await Task.FromResult(result);
    }
}