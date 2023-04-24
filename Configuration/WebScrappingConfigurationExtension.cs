using static AngleSharp.Configuration;

namespace WebScrapping.Configuration;

public static class WebScrappingConfigurationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<IBrowsingContext>(_ =>
                BrowsingContext.New(Default.WithDefaultLoader()))
            .AddSingleton<IDataScrapeService, DataScrapeService>()
            .AddMediatR(config => 
                config.RegisterServicesFromAssemblyContaining<Program>());
}
