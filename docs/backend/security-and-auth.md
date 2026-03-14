# Security And Auth Notes

## Security Priorities

- secure local email/password authentication
- isolate all user data by authenticated local user id
- validate all inputs consistently with FluentValidation and ProblemDetails responses
- never expose password hashes to the client

## JWT Strategy

### Access Token

- short lifetime
- signed by backend
- includes minimal claims such as local user id and email if needed

## Recommended Claims

- `sub` = local user id
- `email`
- `name` if useful

Keep claims minimal and app-specific.

## Authorization Rule

Authentication alone is not enough. Every repository or query path must scope data by current local user id.

Bad pattern:

- lookup by resource id only

Correct pattern:

- lookup by resource id and authenticated user id

## Input Validation

- use FluentValidation validators close to each command or query
- run validation through the MediatR pipeline before handlers execute
- validate DTOs before persistence
- validate route ownership before mutation

## Error Response Notes

- use `ProblemDetails` for auth and domain errors
- use `ValidationProblemDetails` for request validation failures
- keep a stable app-specific `code` extension on problem responses

## Password Notes

- store only hashed passwords
- never log passwords or password hashes
- reject invalid credentials with a generic auth error

## Local Development Notes

- use local configuration values from environment
- document required auth and database settings in `.env.example`
- if HTTPS is awkward locally, keep the flow development-friendly but do not weaken production assumptions in docs

## Logging Guidance

- log auth failures without leaking sensitive token values
- log important domain actions at a useful but not noisy level
- avoid logging raw passwords or password hashes
