using MyFi.Api.Features.Users;

namespace MyFi.Api.Common.Security;

public interface IPasswordService
{
    string HashPassword(User user, string password);

    bool VerifyPassword(User user, string password);
}
