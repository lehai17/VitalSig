using Microsoft.EntityFrameworkCore;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data;

public static class AppDbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();

        var demoEmail = "demo@vitalsig.local";
        var demoUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == demoEmail);

        if (demoUser is not null)
        {
            return;
        }

        dbContext.Users.Add(new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FullName = "Vitalsig Demo User",
            Email = demoEmail,
            PhoneNumber = "0900000000",
            PasswordHash = "demo-password-not-for-production",
            Role = "User",
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }
}
