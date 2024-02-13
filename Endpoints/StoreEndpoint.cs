namespace WebScrapping.Endpoints;

public sealed class StoreEndpoint
{
    public static IResult StoreBlog(
        [FromBody] StoreResource resource,
        [FromServices] IBlogRepository blogRepo)
    {
        blogRepo.SaveBlogAsync(new Blog(resource.Url));

        return Results.Accepted(value: new { message = $"Storing blog {resource.Url}" });
    }
}
