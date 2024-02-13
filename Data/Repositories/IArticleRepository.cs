namespace WebScrapping.Data.Repositories;

public interface IArticleRepository
{
    IEnumerable<Article> GetArticles(Blog blog);
    void AppendArticles(Blog blog, ICollection<Article> articles);
}
