using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Security;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public static class UserTestDataSeeder
{
    public static async Task SeedBaselineAsync(IServiceProvider services)
    {
        await EnsureUserExistsAsync(services, TestUserData.Seeded);
    }

    public static async Task<User> EnsureUserExistsAsync(IServiceProvider services, TestUserData testUser)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyFiDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var existingUser = await dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == testUser.Email);

        if (existingUser is not null)
        {
            return existingUser;
        }

        var user = User.Create(testUser.Email, testUser.DisplayName);
        user.SetPasswordHash(passwordService.HashPassword(user, testUser.Password));

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user;
    }
}
