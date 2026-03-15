using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Expenses;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Expenses.DeleteExpense;

public sealed class DeleteExpenseHandlerTests
{
    [Fact]
    public async Task Handle_DeletesExpense_WhenOwnedByUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var expense = Expense.Create(userId, null, "Coffee", 4m, new DateOnly(2026, 3, 3), null, null, false);
        dbContext.Expenses.Add(expense);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new DeleteExpenseHandler(repository);

        var result = await handler.Handle(new DeleteExpenseCommand(expense.Id, userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(await dbContext.Expenses.ToListAsync());
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExpenseIsMissingForUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new DeleteExpenseHandler(repository);

        var result = await handler.Handle(new DeleteExpenseCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("expense_not_found", result.Error?.Code);
    }
}
