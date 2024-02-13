using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;

namespace WebScrapping.CustomMiddleware;

public static class ScrapeSchedulerConfigurationExtension
{
    public static ISchedulerConfiguration AddScheduler(this IApplicationBuilder builder) =>
        builder.ApplicationServices.UseScheduler(scheduler =>
            scheduler.Schedule<DataScrapeJob>().Cron("00 */8 * * *"));
}