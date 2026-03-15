using MyFi.Api.Features.Expenses;

namespace MyFi.Api.UnitTests.Features.Expenses.ListExpenses;

public sealed class ListExpensesQueryValidatorTests
{
    private readonly ListExpensesQueryValidator _validator = new();

    [Fact]
    public void Validate_ReturnsErrors_WhenPagingSortingOrDateRangeIsInvalid()
    {
        var result = _validator.Validate(new ListExpensesQuery
        {
            Page = 0,
            PageSize = 101,
            SortBy = "title",
            SortDir = "up",
            DateFrom = new DateOnly(2026, 3, 20),
            DateTo = new DateOnly(2026, 3, 10)
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ListExpensesQuery.Page));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ListExpensesQuery.PageSize));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ListExpensesQuery.SortBy));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(ListExpensesQuery.SortDir));
        Assert.Contains(result.Errors, error => error.PropertyName == string.Empty);
    }

    [Fact]
    public void Validate_Succeeds_WhenQueryIsValid()
    {
        var result = _validator.Validate(new ListExpensesQuery
        {
            Page = 1,
            PageSize = 20,
            SortBy = "amount",
            SortDir = "desc",
            DateFrom = new DateOnly(2026, 3, 1),
            DateTo = new DateOnly(2026, 3, 31)
        });

        Assert.True(result.IsValid);
    }
}
