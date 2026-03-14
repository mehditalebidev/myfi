using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Common.Security;

namespace MyFi.Api.Features.Users;

public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IRepository _repository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public LoginHandler(
        IRepository repository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _repository = repository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = User.NormalizeEmail(request.Email);

        var user = await _repository.Query<User>()
            .FirstOrDefaultAsync(candidate => candidate.Email == normalizedEmail, cancellationToken);

        if (user is null || !_passwordService.VerifyPassword(user, request.Password))
        {
            return Result<AuthResponse>.Failure(UserErrors.InvalidCredentials());
        }

        var issuedToken = _tokenService.CreateAccessToken(user);

        return Result<AuthResponse>.Success(new AuthResponse(
            issuedToken.AccessToken,
            issuedToken.ExpiresAt,
            user.ToResponse()));
    }
}
