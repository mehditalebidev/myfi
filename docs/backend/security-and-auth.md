# Security And Auth Notes

## Security Priorities

- secure Google OAuth implementation
- keep provider details behind backend boundary
- isolate all user data by authenticated local user id
- validate all inputs consistently
- never expose refresh token storage details to the client

## JWT Strategy

### Access Token

- short lifetime
- signed by backend
- includes minimal claims such as local user id and email if needed

### Refresh Token

- longer lifetime
- stored hashed in database
- rotated on refresh
- revoked on logout

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

- use FluentValidation in application layer
- validate DTOs before persistence
- validate route ownership before mutation

## OAuth Notes

- use state validation during OAuth flow
- keep Google client secret only on backend
- never rely on frontend-only identity trust for protected API access

## Local Development Notes

- use local configuration values from environment
- document required auth and database settings in `.env.example` later
- if HTTPS is awkward locally, keep the flow development-friendly but do not weaken production assumptions in docs

## Logging Guidance

- log auth failures without leaking sensitive token values
- log important domain actions at a useful but not noisy level
- avoid logging raw refresh tokens or provider secrets
