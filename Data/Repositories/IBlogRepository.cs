namespace WebScrapping.Data.Repositories;

public interface IBlogRepository
{
    Task SaveBlogAsync(Blog blog);
    IEnumerable<Blog> RetrieveBlogs();
}