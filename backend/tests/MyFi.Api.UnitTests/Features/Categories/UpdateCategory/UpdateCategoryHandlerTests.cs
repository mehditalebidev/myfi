using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Categories.UpdateCategory;

public sealed class UpdateCategoryHandlerTests
{
    [Fact]
    public async Task Handle_UpdatesCategory_WhenOwnedByUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Food", null, null);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new UpdateCategoryHandler(repository);

        var result = await handler.Handle(new UpdateCategoryCommand
        {
            Id = category.Id,
            UserId = userId,
            Name = "Dining",
            Color = "#ef4444",
            Icon = "fork-knife"
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Dining", result.Value.Name);
        Assert.Equal("#ef4444", result.Value.Color);

        var savedCategory = await dbContext.Categories.SingleAsync();
        Assert.Equal("Dining", savedCategory.Name);
        Assert.Equal("fork-knife", savedCategory.Icon);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryIsMissingForUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new UpdateCategoryHandler(repository);

        var result = await handler.Handle(new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Name = "Dining"
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_not_found", result.Error?.Code);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenAnotherCategoryAlreadyUsesName()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var firstCategory = Category.Create(userId, "Food", null, null);
        var secondCategory = Category.Create(userId, "Utilities", null, null);
        dbContext.Categories.AddRange(firstCategory, secondCategory);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new UpdateCategoryHandler(repository);

        var result = await handler.Handle(new UpdateCategoryCommand
        {
            Id = secondCategory.Id,
            UserId = userId,
            Name = "food"
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_name_in_use", result.Error?.Code);
    }
}
