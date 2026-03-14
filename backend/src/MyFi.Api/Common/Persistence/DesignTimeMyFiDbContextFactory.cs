using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyFi.Api.Common.Persistence;

public sealed class DesignTimeMyFiDbContextFactory : IDesignTimeDbContextFactory<MyFiDbContext>
{
    public MyFiDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MyFiDatabase")
            ?? "Host=localhost;Port=5432;Database=myfi;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<MyFiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new MyFiDbContext(optionsBuilder.Options);
    }
}
