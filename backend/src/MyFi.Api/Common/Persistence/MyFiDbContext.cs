using Microsoft.EntityFrameworkCore;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.Features.Users;

namespace MyFi.Api.Common.Persistence;

public sealed class MyFiDbContext : DbContext
{
    public MyFiDbContext(DbContextOptions<MyFiDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Expense> Expenses => Set<Expense>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFiDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
