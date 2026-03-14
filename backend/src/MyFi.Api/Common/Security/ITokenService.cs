using MyFi.Api.Features.Users;

namespace MyFi.Api.Common.Security;

public interface ITokenService
{
    IssuedToken CreateAccessToken(User user);
}
