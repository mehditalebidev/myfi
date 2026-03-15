using FluentValidation;

namespace MyFi.Api.Features.Expenses;

public sealed class ListExpensesQueryValidator : AbstractValidator<ListExpensesQuery>
{
    private static readonly string[] AllowedSortBy = ["expenseDate", "amount"];
    private static readonly string[] AllowedSortDir = ["asc", "desc"];

    public ListExpensesQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.SortBy)
            .Must(sortBy => sortBy is null || AllowedSortBy.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: expenseDate, amount.");

        RuleFor(query => query.SortDir)
            .Must(sortDir => sortDir is null || AllowedSortDir.Contains(sortDir, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortDir must be one of: asc, desc.");

        RuleFor(query => query)
            .Must(query => query.DateFrom is null || query.DateTo is null || query.DateFrom <= query.DateTo)
            .WithMessage("DateFrom must be on or before DateTo.");
    }
}
