using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;

namespace MyFi.Api.UnitTests.Support;

internal static class InMemoryDbContextFactory
{
    public static MyFiDbContext Create()
    {
        var options = new DbContextOptionsBuilder<MyFiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MyFiDbContext(options);
    }
}
