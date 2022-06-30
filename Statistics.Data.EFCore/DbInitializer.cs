using Microsoft.EntityFrameworkCore;

namespace Statistics.Data.EFCore;

public static class DbInitializer
{
    public static async Task Initialize(StatisticsDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}