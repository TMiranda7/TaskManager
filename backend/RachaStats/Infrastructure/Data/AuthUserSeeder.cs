using Microsoft.EntityFrameworkCore;
using RachaStats.Application.Auth;
using RachaStats.Domain.Entities;

namespace RachaStats.Infrastructure.Data;

public static class AuthUserSeeder
{
    private const string SeedUsersSectionName = "Auth:SeedUsers";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var seedUsers = configuration
            .GetSection(SeedUsersSectionName)
            .Get<List<SeedUserSettings>>() ?? [];

        if (seedUsers.Count == 0)
        {
            return;
        }

        var hasChanges = false;

        foreach (var seedUser in seedUsers)
        {
            if (string.IsNullOrWhiteSpace(seedUser.Username) || string.IsNullOrWhiteSpace(seedUser.Password))
            {
                continue;
            }

            var username = seedUser.Username.Trim();
            var exists = await context.AppUsers.AnyAsync(user => user.Username == username);

            if (exists)
            {
                continue;
            }

            await context.AppUsers.AddAsync(new AppUser
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = PasswordHasher.ComputeSha256(seedUser.Password),
                Role = string.IsNullOrWhiteSpace(seedUser.Role) ? "User" : seedUser.Role.Trim(),
                CreatedAt = DateTime.UtcNow
            });

            hasChanges = true;
        }

        if (hasChanges)
        {
            await context.SaveChangesAsync();
        }
    }

    private sealed class SeedUserSettings
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Role { get; init; } = "User";
    }
}
