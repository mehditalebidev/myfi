using MyFi.Api.Features.Expenses;

namespace MyFi.Api.UnitTests.Features.Expenses.Domain;

public sealed class ExpenseTests
{
    [Fact]
    public void Create_TrimsRequiredAndOptionalValues()
    {
        var expense = Expense.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "  Supermarket  ",
            84.50m,
            new DateOnly(2026, 3, 12),
            "  card  ",
            "  weekly shopping  ",
            false);

        Assert.Equal("Supermarket", expense.Title);
        Assert.Equal("card", expense.PaymentMethod);
        Assert.Equal("weekly shopping", expense.Note);
    }

    [Fact]
    public void Create_NormalizesBlankOptionalValuesToNull()
    {
        var expense = Expense.Create(
            Guid.NewGuid(),
            null,
            "Coffee",
            4.25m,
            new DateOnly(2026, 3, 13),
            "   ",
            string.Empty,
            false);

        Assert.Null(expense.PaymentMethod);
        Assert.Null(expense.Note);
    }

    [Fact]
    public void Update_RefreshesUpdatedAt_AndUpdatesFields()
    {
        var expense = Expense.Create(
            Guid.NewGuid(),
            null,
            "Lunch",
            12.00m,
            new DateOnly(2026, 3, 10),
            null,
            null,
            false);

        var originalUpdatedAt = expense.UpdatedAt;
        var categoryId = Guid.NewGuid();

        Thread.Sleep(5);

        expense.Update(
            categoryId,
            "  Team lunch  ",
            18.75m,
            new DateOnly(2026, 3, 11),
            "  card  ",
            "  reimbursable  ",
            true);

        Assert.Equal(categoryId, expense.CategoryId);
        Assert.Equal("Team lunch", expense.Title);
        Assert.Equal(18.75m, expense.Amount);
        Assert.Equal(new DateOnly(2026, 3, 11), expense.ExpenseDate);
        Assert.Equal("card", expense.PaymentMethod);
        Assert.Equal("reimbursable", expense.Note);
        Assert.True(expense.IsRecurring);
        Assert.True(expense.UpdatedAt > originalUpdatedAt);
    }
}
