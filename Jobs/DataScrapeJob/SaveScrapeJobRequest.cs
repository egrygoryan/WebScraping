using MediatR.Pipeline;

namespace WebScrapping.Jobs.DataScrapeJob;

public sealed class SaveScrapeJobRequest(IArticleRepository articleRepo) : IRequestPostProcessor<ScrapeJobRequest, ICollection<Article>>
{
    public Task Process(ScrapeJobRequest request, ICollection<Article> response, CancellationToken cancellationToken)
    {
        if (!request.Blog.IsStored)
        {
            articleRepo.AppendArticles(request.Blog, response);
            return Task.CompletedTask;
        }

        var storedArticles = articleRepo.GetArticles(request.Blog);
        var newArticles = response.Except(storedArticles).ToHashSet();

        if (newArticles.Count != 0)
        {
            foreach (var article in newArticles)
            {
                article.RemoveTag();
            }
            articleRepo.AppendArticles(request.Blog, newArticles);
        }

        return Task.CompletedTask;
    }
}
