using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyFi.Api.Common.Persistence;

namespace MyFi.Api.IntegrationTests;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"myfi-api-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<MyFiDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<MyFiDbContext>>();
            services.RemoveAll<MyFiDbContext>();

            services.AddDbContext<MyFiDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
