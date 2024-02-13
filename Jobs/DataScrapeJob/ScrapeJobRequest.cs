namespace WebScrapping.Jobs.DataScrapeJob;

public record ScrapeJobRequest(Blog Blog, int Range) : IRequest<ICollection<Article>>;
