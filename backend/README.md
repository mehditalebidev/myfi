# Backend Workspace

This directory is reserved for the MyFi backend application.

## Current State

- The backend solution now centers on a single ASP.NET Core API project with an organized vertical-slice layout.
- Local email/password auth is bootstrapped with signup, login, and `users/me` endpoints.
- `docs/` is for backend-specific implementation notes and agent-readable guidance.
- `src/` contains the API project; feature files stay close together by slice, with MediatR handlers and FluentValidation validators living beside their endpoints.

## Read First

Before backend implementation work, read:

1. `docs/shared/README.md`
2. `docs/shared/product-scope.md`
3. `docs/shared/domain-model.md`
4. `docs/shared/auth-flow.md`
5. `docs/shared/api-contract.md`
6. `docs/backend/backend-architecture.md`
7. `docs/backend/backend-endpoints.md`
8. `docs/backend/security-and-auth.md`
9. `docs/coordination/BOARD.md`
10. `docs/WORKLOG.md`
11. `backend/docs/README.md`

## Solution Layout

```text
backend/
  MyFi.sln
  src/
    MyFi.Api/
  tests/
    MyFi.Api.IntegrationTests/
```

## Useful Commands

Run from `backend/`:

- `dotnet restore MyFi.sln`
- `dotnet build MyFi.sln`
- `dotnet test MyFi.sln`
- `dotnet run --project src/MyFi.Api/MyFi.Api.csproj`
- `dotnet ef database update --project src/MyFi.Api/MyFi.Api.csproj --startup-project src/MyFi.Api/MyFi.Api.csproj`

Run from the repo root:

- `docker compose up --build`

## Notes

- Keep backend-only working notes under `backend/docs/`.
- Use `backend/docs/local-development.md` for local runtime details.
- Before starting a feature, confirm the active story or bug in `docs/coordination/BOARD.md`.
- Keep slice-local controllers, commands, validators, handlers, DTOs, entities, and EF configurations together under `backend/src/MyFi.Api/Features/`.
