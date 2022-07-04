using Statistics.Data.EFCore;

namespace Statistics.Api.Extensions;

public static class ServiceProviderExtension
{
    public static async Task CreateDbIfNotExists(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StatisticsDbContext>();

            await DbInitializer.Initialize(context);
        }
    }
}