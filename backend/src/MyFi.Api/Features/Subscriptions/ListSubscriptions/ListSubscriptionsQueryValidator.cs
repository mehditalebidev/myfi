using FluentValidation;

namespace MyFi.Api.Features.Subscriptions;

public sealed class ListSubscriptionsQueryValidator : AbstractValidator<ListSubscriptionsQuery>
{
    private static readonly string[] AllowedSortBy = ["renewalDate", "amount"];
    private static readonly string[] AllowedSortDirections = ["asc", "desc"];

    public ListSubscriptionsQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(query => query.SortBy)
            .Must(sortBy => sortBy is null || AllowedSortBy.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: renewalDate, amount.");

        RuleFor(query => query.SortDir)
            .Must(sortDir => sortDir is null || AllowedSortDirections.Contains(sortDir, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortDir must be one of: asc, desc.");
    }
}
