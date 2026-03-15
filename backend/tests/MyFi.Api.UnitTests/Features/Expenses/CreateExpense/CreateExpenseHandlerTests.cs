using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Expenses.CreateExpense;

public sealed class CreateExpenseHandlerTests
{
    [Fact]
    public async Task Handle_CreatesExpense_WhenCategoryBelongsToUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Groceries", null, null);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateExpenseHandler(repository);

        var result = await handler.Handle(new CreateExpenseCommand
        {
            UserId = userId,
            CategoryId = category.Id,
            Title = "  Supermarket  ",
            Amount = 84.50m,
            ExpenseDate = new DateOnly(2026, 3, 12),
            PaymentMethod = "  card  ",
            Note = "  weekly shopping  ",
            IsRecurring = false
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Supermarket", result.Value.Title);
        Assert.Equal("Groceries", result.Value.CategoryName);
        Assert.Equal("card", result.Value.PaymentMethod);

        var savedExpense = await dbContext.Expenses.SingleAsync();
        Assert.Equal(userId, savedExpense.UserId);
        Assert.Equal(category.Id, savedExpense.CategoryId);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryDoesNotBelongToUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var category = Category.Create(Guid.NewGuid(), "Travel", null, null);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateExpenseHandler(repository);

        var result = await handler.Handle(new CreateExpenseCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = category.Id,
            Title = "Flight",
            Amount = 300m,
            ExpenseDate = new DateOnly(2026, 3, 20)
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_not_found", result.Error?.Code);
        Assert.Empty(await dbContext.Expenses.ToListAsync());
    }
}
