namespace MyFi.Api.Features.Users;

public sealed record AuthResponse(string AccessToken, DateTime ExpiresAt, UserResponse User);
