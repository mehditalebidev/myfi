using Microsoft.Extensions.Options;
using MyFi.Api.Common.Security;
using MyFi.Api.Features.Users;
using System.IdentityModel.Tokens.Jwt;

namespace MyFi.Api.UnitTests.Common.Security;

public sealed class TokenServiceTests
{
    [Fact]
    public void CreateAccessToken_ReturnsSignedTokenWithExpectedClaims()
    {
        var options = Options.Create(new JwtOptions
        {
            Issuer = "myfi-tests",
            Audience = "myfi-client",
            SigningKey = "development-signing-key-with-32-characters-minimum",
            AccessTokenMinutes = 15
        });
        var service = new TokenService(options);
        var user = User.Create("Mehdi@example.com", " Mehdi ");
        var before = DateTime.UtcNow;

        var issuedToken = service.CreateAccessToken(user);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(issuedToken.AccessToken);
        var after = DateTime.UtcNow;

        Assert.Equal("myfi-tests", jwt.Issuer);
        Assert.Equal("myfi-client", Assert.Single(jwt.Audiences));
        Assert.Equal(user.Id.ToString(), jwt.Subject);
        Assert.Equal(user.Email, jwt.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Email).Value);
        Assert.Equal(user.DisplayName, jwt.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Name).Value);
        Assert.InRange(issuedToken.ExpiresAt, before.AddMinutes(15).AddSeconds(-5), after.AddMinutes(15).AddSeconds(5));
        Assert.InRange(
            Math.Abs((issuedToken.ExpiresAt - jwt.ValidTo).TotalSeconds),
            0,
            1);
    }
}
