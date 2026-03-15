using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Categories.CreateCategory;

public sealed class CreateCategoryHandlerTests
{
    [Fact]
    public async Task Handle_CreatesCategory_WhenNameIsAvailable()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new CreateCategoryHandler(repository);
        var userId = Guid.NewGuid();

        var result = await handler.Handle(new CreateCategoryCommand
        {
            UserId = userId,
            Name = "  Groceries  ",
            Color = "  #3b82f6  ",
            Icon = "  shopping-cart  "
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Groceries", result.Value.Name);
        Assert.Equal("#3b82f6", result.Value.Color);
        Assert.Equal("shopping-cart", result.Value.Icon);

        var savedCategory = await dbContext.Categories.SingleAsync();
        Assert.Equal(userId, savedCategory.UserId);
        Assert.Equal("Groceries", savedCategory.Name);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenNameAlreadyExistsForSameUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        dbContext.Categories.Add(Category.Create(userId, "Groceries", null, null));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateCategoryHandler(repository);

        var result = await handler.Handle(new CreateCategoryCommand
        {
            UserId = userId,
            Name = "groceries"
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_name_in_use", result.Error?.Code);
        Assert.Equal(1, await dbContext.Categories.CountAsync());
    }

    [Fact]
    public async Task Handle_AllowsSameNameForDifferentUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        dbContext.Categories.Add(Category.Create(Guid.NewGuid(), "Groceries", null, null));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateCategoryHandler(repository);

        var result = await handler.Handle(new CreateCategoryCommand
        {
            UserId = Guid.NewGuid(),
            Name = "Groceries"
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, await dbContext.Categories.CountAsync());
    }
}
