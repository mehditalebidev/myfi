using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Expenses.GetExpense;

public sealed class GetExpenseHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsOwnedExpense()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Food", null, null);
        var expense = Expense.Create(userId, category.Id, "Lunch", 14m, new DateOnly(2026, 3, 2), null, null, false);
        dbContext.Categories.Add(category);
        dbContext.Expenses.Add(expense);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new GetExpenseHandler(repository);

        var result = await handler.Handle(new GetExpenseQuery(expense.Id, userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Lunch", result.Value.Title);
        Assert.Equal("Food", result.Value.CategoryName);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExpenseBelongsToDifferentUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var expense = Expense.Create(Guid.NewGuid(), null, "Private", 10m, new DateOnly(2026, 3, 2), null, null, false);
        dbContext.Expenses.Add(expense);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new GetExpenseHandler(repository);

        var result = await handler.Handle(new GetExpenseQuery(expense.Id, Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("expense_not_found", result.Error?.Code);
    }
}
