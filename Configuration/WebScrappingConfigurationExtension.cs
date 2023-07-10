using static AngleSharp.Configuration;
using MediatR.Pipeline;

namespace WebScrapping.Configuration;

public static class WebScrappingConfigurationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton(_ =>
                BrowsingContext.New(Default.WithDefaultLoader()))
            .AddSingleton<IDataScrapeService, DataScrapeService>()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<Program>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestExceptionProcessorBehavior<,>));
            });
}