using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Users;

public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<Result<UserResponse>>;
