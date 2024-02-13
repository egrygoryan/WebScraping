namespace WebScrapping.Data.Repositories;

public sealed class ArticleRepository : IArticleRepository
{
    private static readonly Dictionary<Blog, ICollection<Article>> _scrapedArticles = [];
    private static KeyNotFoundException BlogNotFound => new("Blog not found");

    public IEnumerable<Article> GetArticles(Blog blog)
    {
        if (_scrapedArticles.TryGetValue(blog, out var existingArticles))
        {
            return existingArticles;
        }

        throw BlogNotFound;
    }

    public void AppendArticles(Blog blog, ICollection<Article> articles)
    {
        if (!_scrapedArticles.TryGetValue(blog, out var existingArticles))
        {
            existingArticles = new HashSet<Article>();
            _scrapedArticles.Add(blog, existingArticles);
            blog.Store();
        }

        foreach (var article in articles)
        {
            existingArticles.Add(article);
        }
    }

    public IEnumerable<Article> GetNewArticles(FilterNewArticlesRequest filter)
    {
        return _scrapedArticles
            .Values
            .SelectMany(articles => articles
                .Where(x => x.PublishedDate >= filter.StartDate
                    && x.PublishedDate <= filter.EndDate
                    && x.Originality == Tag.New))
            .ToList();
    }
}
