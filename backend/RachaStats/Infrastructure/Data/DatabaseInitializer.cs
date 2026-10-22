using Microsoft.EntityFrameworkCore;

namespace RachaStats.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (configuration.GetValue<bool>("DatabaseInitialization:ApplyMigrations"))
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
        }

        if (configuration.GetValue<bool>("DatabaseInitialization:SeedUsers"))
        {
            await AuthUserSeeder.SeedAsync(scope.ServiceProvider);
        }
    }
}
