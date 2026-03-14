# Backend Local Development

## What Exists

- ASP.NET Core Web API using an organized vertical-slice layout inside `MyFi.Api`
- PostgreSQL wiring through EF Core and Npgsql
- automatic migration application on startup
- local auth endpoints for signup and login plus an authenticated `users/me` endpoint
- MediatR handlers, FluentValidation validators, and ProblemDetails-based error responses

## Environment

Copy the root `.env.example` to `.env` when you want local overrides.

Important values:

- `POSTGRES_DB`
- `POSTGRES_USER`
- `POSTGRES_PASSWORD`
- `POSTGRES_PORT`
- `API_PORT`
- `CONNECTIONSTRINGS__MYFIDATABASE`
- `JWT__ISSUER`
- `JWT__AUDIENCE`
- `JWT__SIGNINGKEY`
- `JWT__ACCESSTOKENMINUTES`

## Run With Docker Compose

From the repository root:

```bash
docker compose up --build
```

This starts:

- PostgreSQL on the configured host port
- the backend API on `http://localhost:${API_PORT}`

## Run Integration Tests

From `backend/`:

```bash
dotnet test MyFi.sln
```

Integration tests use `WebApplicationFactory` with a PostgreSQL Testcontainer. Docker must be running before the test suite starts. The test fixture boots a dedicated PostgreSQL container, applies migrations through the API startup path, and seeds baseline integration-test data before requests run.

Test support is organized under `backend/tests/MyFi.Api.IntegrationTests/Support/`:

- `Infrastructure/` for the test container and WebApplicationFactory wiring
- `Seeding/` for baseline seed orchestration
- `Users/` for user-specific test data helpers and seeders

## Auth Endpoints

### `POST /api/auth/signup`

Request:

```json
{
  "email": "mehdi@example.com",
  "displayName": "Mehdi",
  "password": "Password123!"
}
```

Returns a JWT access token, token expiry, and the created user.

### `POST /api/auth/login`

Request:

```json
{
  "email": "mehdi@example.com",
  "password": "Password123!"
}
```

Validation:

- `email` is required and must be valid
- `displayName` is required for signup and must be between 2 and 100 characters
- `password` must be between 8 and 100 characters
- validation failures return `application/problem+json` with field errors

Success response:

```json
{
  "accessToken": "jwt",
  "expiresAt": "2026-03-14T12:00:00Z",
  "user": {
    "id": "uuid",
    "email": "mehdi@example.com",
    "displayName": "Mehdi"
  }
}
```

### `GET /api/users/me`

Requires `Authorization: Bearer <token>` and returns the authenticated user profile.

## Database Notes

- current bootstrap table: `users`
- migrations live in `backend/src/MyFi.Api/Common/Persistence/Migrations/`
- `backend/src/MyFi.Api/Features/Users/` is the reference slice for keeping endpoints, commands, validators, handlers, DTOs, and EF mapping close together
- integration tests use a separate PostgreSQL containerized database seeded with a baseline local user before test execution
