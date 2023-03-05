namespace WebScrapping.Configuration;

public static class WebScrappingConfigurationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddSingleton<IDataScrapeService, DataScrapeService>()
                .AddScoped<IValidateService, ValidateService>();
}
