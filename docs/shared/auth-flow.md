# Authentication Flow

## Chosen Direction

- Sign-in provider for phase 1: Google
- App auth style: SPA uses backend-issued JWTs
- OAuth handling: backend-managed Google OAuth code flow
- Goal: keep auth provider-agnostic while only implementing Google first

## Why This Direction

- The backend keeps provider-specific auth logic encapsulated.
- The frontend only depends on app-level auth APIs, not Google-specific token behavior.
- Adding GitHub, Microsoft, or another provider later becomes easier because provider logic stays behind the backend auth layer.

## High-Level Flow

1. User clicks `Continue with Google` in the frontend.
2. Frontend sends the user to backend auth start endpoint.
3. Backend redirects to Google OAuth consent screen.
4. Google redirects back to backend callback endpoint with authorization code.
5. Backend exchanges the code with Google.
6. Backend reads provider user info.
7. Backend creates or links local `User` and `AuthIdentity`.
8. Backend issues app `accessToken` and `refreshToken`.
9. Backend redirects the browser back to the frontend callback route.
10. Frontend completes sign-in state and begins authenticated API usage.

## Token Model

### Access Token

- Short-lived JWT
- Sent on API requests as `Authorization: Bearer <token>`
- Contains app-level claims such as local `userId`

### Refresh Token

- Longer-lived token for renewing the access token
- Stored and rotated securely
- Should be revocable
- Backed by a server-side persisted hashed token record

## Recommended Security Rules

- Never expose Google provider tokens to the frontend unless there is a real product need
- Store only hashed refresh tokens in the database
- Rotate refresh tokens on refresh
- Revoke refresh tokens on logout
- Keep JWT payload minimal
- Use HTTPS in real deployments

## Auth Endpoints

- `POST /auth/google/start`
- `GET /auth/google/callback`
- `POST /auth/refresh`
- `POST /auth/logout`
- `GET /users/me`

## Future Extensibility

To add another provider later:

- add provider configuration
- add provider OAuth start and callback handlers
- map provider claims to the same local `User` / `AuthIdentity` model
- keep the frontend auth state model unchanged
