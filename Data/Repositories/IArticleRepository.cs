namespace WebScrapping.Data.Repositories;

public interface IArticleRepository
{
    IEnumerable<Article> GetArticles(Blog blog);
    IEnumerable<Article> GetNewArticles(FilterNewArticlesRequest filter);
    void AppendArticles(Blog blog, ICollection<Article> articles);
}
