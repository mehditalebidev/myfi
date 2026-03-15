using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed record DeleteCategoryCommand(Guid Id, Guid UserId) : IRequest<Result>;
