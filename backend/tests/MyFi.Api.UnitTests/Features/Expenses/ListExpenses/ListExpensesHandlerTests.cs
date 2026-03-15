using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Expenses.ListExpenses;

public sealed class ListExpensesHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCurrentUsersExpenses_WithFilteringSortingAndPaging()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var groceries = Category.Create(userId, "Groceries", null, null);
        var transport = Category.Create(userId, "Transport", null, null);
        dbContext.Categories.AddRange(groceries, transport, Category.Create(Guid.NewGuid(), "Ignored", null, null));
        dbContext.Expenses.AddRange(
            Expense.Create(userId, groceries.Id, "Weekly shop", 84.50m, new DateOnly(2026, 3, 12), "card", "market", false),
            Expense.Create(userId, transport.Id, "Train ticket", 12.30m, new DateOnly(2026, 3, 11), "card", null, false),
            Expense.Create(userId, groceries.Id, "Coffee beans", 18.20m, new DateOnly(2026, 3, 9), null, "store", false),
            Expense.Create(Guid.NewGuid(), groceries.Id, "Foreign", 999m, new DateOnly(2026, 3, 10), null, null, false));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new ListExpensesHandler(repository);

        var result = await handler.Handle(new ListExpensesQuery
        {
            UserId = userId,
            Search = "shop",
            CategoryId = groceries.Id,
            SortBy = "amount",
            SortDir = "desc",
            Page = 1,
            PageSize = 1
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
        Assert.Single(result.Value.Items);
        Assert.Equal("Weekly shop", result.Value.Items[0].Title);
        Assert.Equal("Groceries", result.Value.Items[0].CategoryName);
    }

    [Fact]
    public async Task Handle_DefaultsToExpenseDateDescending()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        dbContext.Expenses.AddRange(
            Expense.Create(userId, null, "Older", 10m, new DateOnly(2026, 3, 1), null, null, false),
            Expense.Create(userId, null, "Newer", 10m, new DateOnly(2026, 3, 5), null, null, false));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new ListExpensesHandler(repository);

        var result = await handler.Handle(new ListExpensesQuery { UserId = userId }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { "Newer", "Older" }, result.Value.Items.Select(item => item.Title).ToArray());
    }
}
