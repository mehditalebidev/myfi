using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Categories.ListCategories;

public sealed class ListCategoriesHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCurrentUsersCategoriesOrderedByName()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        dbContext.Categories.AddRange(
            Category.Create(userId, "Utilities", null, null),
            Category.Create(Guid.NewGuid(), "Ignored", null, null),
            Category.Create(userId, "Food", null, null));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new ListCategoriesHandler(repository);

        var result = await handler.Handle(new ListCategoriesQuery(userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { "Food", "Utilities" }, result.Value.Select(category => category.Name).ToArray());
    }
}
