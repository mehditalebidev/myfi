using Microsoft.EntityFrameworkCore;
using MyFi.Api.Features.Users;

namespace MyFi.Api.Common.Persistence;

public sealed class MyFiDbContext : DbContext
{
    public MyFiDbContext(DbContextOptions<MyFiDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFiDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
