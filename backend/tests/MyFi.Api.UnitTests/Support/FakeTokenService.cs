using MyFi.Api.Common.Security;
using MyFi.Api.Features.Users;

namespace MyFi.Api.UnitTests.Support;

internal sealed class FakeTokenService : ITokenService
{
    public static readonly DateTime FixedExpiresAt = new(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public IssuedToken CreateAccessToken(User user)
    {
        return new IssuedToken($"token-for-{user.Id}", FixedExpiresAt);
    }
}
