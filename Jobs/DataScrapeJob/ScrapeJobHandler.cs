namespace WebScrapping.Jobs.DataScrapeJob;

public sealed class ScrapeJobHandler(
    IDataScrapeService dataScrapeService) : IRequestHandler<ScrapeJobRequest, ICollection<Article>>
{
    public async Task<ICollection<Article>> Handle(ScrapeJobRequest request, CancellationToken cancellationToken)
    {
        var articles = await dataScrapeService.ScrapeBlog(request.Blog.Url, request.Range);

        var scrapedArticles = articles
            .Value
            .Select(x => new Article(
                origin: x.Origin,
                author: x.Author,
                title: x.Title,
                description: x.Description,
                publishedDate: DateTime.Parse(x.PublishedDate)))
            .ToList();

        return scrapedArticles;
    }
}
