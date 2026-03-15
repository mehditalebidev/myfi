using MyFi.Api.Common.Security;
using MyFi.Api.Features.Users;

namespace MyFi.Api.UnitTests.Support;

internal sealed class FakePasswordService : IPasswordService
{
    public string HashPassword(User user, string password)
    {
        return $"hashed::{password}";
    }

    public bool VerifyPassword(User user, string password)
    {
        return user.PasswordHash == HashPassword(user, password);
    }
}
