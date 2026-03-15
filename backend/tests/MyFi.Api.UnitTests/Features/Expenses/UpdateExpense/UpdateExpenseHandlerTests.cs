using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Expenses.UpdateExpense;

public sealed class UpdateExpenseHandlerTests
{
    [Fact]
    public async Task Handle_UpdatesOwnedExpense()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Bills", null, null);
        var expense = Expense.Create(userId, null, "Electricity", 95m, new DateOnly(2026, 3, 8), null, null, false);
        dbContext.Categories.Add(category);
        dbContext.Expenses.Add(expense);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new UpdateExpenseHandler(repository);

        var result = await handler.Handle(new UpdateExpenseCommand
        {
            Id = expense.Id,
            UserId = userId,
            CategoryId = category.Id,
            Title = "  Electricity bill  ",
            Amount = 102.40m,
            ExpenseDate = new DateOnly(2026, 3, 9),
            PaymentMethod = "  bank transfer  ",
            Note = "  paid early  ",
            IsRecurring = true
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Electricity bill", result.Value.Title);
        Assert.Equal("Bills", result.Value.CategoryName);
        Assert.Equal("bank transfer", result.Value.PaymentMethod);

        var savedExpense = await dbContext.Expenses.SingleAsync();
        Assert.Equal(category.Id, savedExpense.CategoryId);
        Assert.Equal(102.40m, savedExpense.Amount);
        Assert.True(savedExpense.IsRecurring);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExpenseIsMissingForUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new UpdateExpenseHandler(repository);

        var result = await handler.Handle(new UpdateExpenseCommand
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Title = "Missing",
            Amount = 10m,
            ExpenseDate = new DateOnly(2026, 3, 1)
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("expense_not_found", result.Error?.Code);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryIsMissingForUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var expense = Expense.Create(userId, null, "Lunch", 15m, new DateOnly(2026, 3, 3), null, null, false);
        dbContext.Expenses.Add(expense);
        dbContext.Categories.Add(Category.Create(Guid.NewGuid(), "Foreign", null, null));
        await dbContext.SaveChangesAsync();

        var foreignCategoryId = await dbContext.Categories.Select(category => category.Id).SingleAsync();
        var repository = new Repository(dbContext);
        var handler = new UpdateExpenseHandler(repository);

        var result = await handler.Handle(new UpdateExpenseCommand
        {
            Id = expense.Id,
            UserId = userId,
            CategoryId = foreignCategoryId,
            Title = "Lunch",
            Amount = 15m,
            ExpenseDate = new DateOnly(2026, 3, 3)
        }, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("category_not_found", result.Error?.Code);
    }
}
