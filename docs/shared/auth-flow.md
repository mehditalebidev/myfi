# Authentication Flow

## Chosen Direction

- Sign-in provider for the current backend bootstrap: local email/password
- App auth style: SPA uses backend-issued JWTs
- Goal: keep auth simple during scaffold work and leave room to add external providers later

## Why This Direction

- The backend owns password hashing and token issuance.
- The frontend only depends on app-level auth APIs.
- The current approach is smaller and faster to iterate on while the rest of the product scaffold is still taking shape.

## High-Level Flow

1. User submits signup or login in the frontend.
2. Frontend sends credentials to the backend auth endpoint.
3. Backend validates the request.
4. On signup, backend creates a local `User` with a hashed password.
5. On login, backend verifies the stored password hash.
6. Backend issues an app `accessToken`.
7. Frontend stores the access token and begins authenticated API usage.

## Token Model

### Access Token

- Short-lived JWT
- Sent on API requests as `Authorization: Bearer <token>`
- Contains app-level claims such as local `userId`

## Recommended Security Rules

- Store only hashed passwords in the database
- Keep JWT payload minimal
- Use HTTPS in real deployments

## Auth Endpoints

- `POST /api/v1/auth/signup`
- `POST /api/v1/auth/login`
- `GET /api/v1/users/me`

## Future Extensibility

To add another provider later:

- add provider configuration
- add provider-specific auth endpoints or callbacks
- map provider claims to the same local `User` model
- keep the frontend auth state model unchanged where possible
