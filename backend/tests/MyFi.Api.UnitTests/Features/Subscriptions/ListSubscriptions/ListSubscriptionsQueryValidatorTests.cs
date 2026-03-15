using FluentValidation.TestHelper;
using MyFi.Api.Features.Subscriptions;

namespace MyFi.Api.UnitTests.Features.Subscriptions.ListSubscriptions;

public sealed class ListSubscriptionsQueryValidatorTests
{
    private readonly ListSubscriptionsQueryValidator _validator = new();

    [Fact]
    public void Validate_ReturnsErrors_ForInvalidPagingAndSorting()
    {
        var result = _validator.TestValidate(new ListSubscriptionsQuery
        {
            Page = 0,
            PageSize = 101,
            SortBy = "name",
            SortDir = "up"
        });

        result.ShouldHaveValidationErrorFor(query => query.Page);
        result.ShouldHaveValidationErrorFor(query => query.PageSize);
        result.ShouldHaveValidationErrorFor(query => query.SortBy);
        result.ShouldHaveValidationErrorFor(query => query.SortDir);
    }

    [Fact]
    public void Validate_Succeeds_ForValidQuery()
    {
        var result = _validator.TestValidate(new ListSubscriptionsQuery
        {
            Page = 1,
            PageSize = 20,
            SortBy = "renewalDate",
            SortDir = "desc"
        });

        result.ShouldNotHaveAnyValidationErrors();
    }
}
