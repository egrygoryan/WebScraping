namespace WebScrapping.Data.Repositories;

public sealed class BlogRepository : IBlogRepository
{
    private static readonly HashSet<Blog> blogs = [];

    public async Task SaveBlogAsync(Blog blog)
    {
        await Task.Run(() =>
        {
            lock (blogs)
            {
                blogs.Add(blog);
            }
        });
    }

    public IEnumerable<Blog> RetrieveBlogs()
    {
        lock (blogs)
            return blogs;
    }
}
