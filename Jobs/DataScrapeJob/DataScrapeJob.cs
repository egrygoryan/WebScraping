using Coravel.Invocable;

namespace WebScrapping.Jobs.DataScrapeJob;

public class DataScrapeJob(
    IBlogRepository blogRepo,
    IMediator mediator) : IInvocable
{
    public async Task Invoke()
    {
        var blogs = blogRepo.RetrieveBlogs();
        var awaiter = blogs.Select(x => mediator.Send(new ScrapeJobRequest(x, 10)));

        await Task.WhenAll(awaiter);
    }
}
