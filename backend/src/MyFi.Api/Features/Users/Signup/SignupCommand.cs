using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Users;

public sealed record SignupCommand : IRequest<Result<AuthResponse>>
{
    public string Email { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
