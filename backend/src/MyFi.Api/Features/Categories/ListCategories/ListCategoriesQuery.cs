using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed record ListCategoriesQuery(Guid UserId) : IRequest<Result<IReadOnlyList<CategoryResponse>>>;
