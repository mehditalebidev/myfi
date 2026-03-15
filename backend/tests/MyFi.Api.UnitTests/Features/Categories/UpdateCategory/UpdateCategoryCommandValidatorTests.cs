using MyFi.Api.Features.Categories;

namespace MyFi.Api.UnitTests.Features.Categories.UpdateCategory;

public sealed class UpdateCategoryCommandValidatorTests
{
    private readonly UpdateCategoryCommandValidator _validator = new();

    [Fact]
    public void Validate_ReturnsNoErrors_ForValidCommand()
    {
        var command = new UpdateCategoryCommand
        {
            Name = "Utilities",
            Color = "#22c55e",
            Icon = "bolt"
        };

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ReturnsExpectedErrors_ForInvalidCommand()
    {
        var command = new UpdateCategoryCommand
        {
            Name = "   ",
            Color = new string('c', 33),
            Icon = new string('i', 101)
        };

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Name is required.");
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Color must be 32 characters or fewer.");
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Icon must be 100 characters or fewer.");
    }
}
