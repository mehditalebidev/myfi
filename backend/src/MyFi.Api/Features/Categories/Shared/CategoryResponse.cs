namespace MyFi.Api.Features.Categories;

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    string? Color,
    string? Icon,
    DateTime CreatedAt,
    DateTime UpdatedAt);
