using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Categories;

public sealed record CreateCategoryCommand : IRequest<Result<CategoryResponse>>
{
    [JsonIgnore]
    public Guid UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Color { get; init; }

    public string? Icon { get; init; }
}
