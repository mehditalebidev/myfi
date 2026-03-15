using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Categories.DeleteCategory;

public sealed class DeleteCategoryHandlerTests
{
    [Fact]
    public async Task Handle_DeletesCategory_WhenOwnedByUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Groceries", null, null);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new DeleteCategoryHandler(repository);

        var result = await handler.Handle(new DeleteCategoryCommand(category.Id, userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(await dbContext.Categories.ToListAsync());
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryIsMissingForUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new DeleteCategoryHandler(repository);

        var result = await handler.Handle(new DeleteCategoryCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_not_found", result.Error?.Code);
    }
}
