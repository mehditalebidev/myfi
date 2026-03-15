using FluentValidation;

namespace MyFi.Api.Features.Categories;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must be 100 characters or fewer.");

        RuleFor(command => command.Color)
            .MaximumLength(32)
            .WithMessage("Color must be 32 characters or fewer.");

        RuleFor(command => command.Icon)
            .MaximumLength(100)
            .WithMessage("Icon must be 100 characters or fewer.");
    }
}
