# MyFi

MyFi is a personal finance and subscription tracker planned as a full-stack portfolio project.

## Current State

- This repository is in planning and scaffold setup stage.
- The implementation docs live under `docs/`.
- The frontend workspace is still a placeholder.
- The backend workspace now contains a PostgreSQL-backed ASP.NET Core API using an organized vertical-slice structure with MediatR, FluentValidation, and local signup, login, and `users/me` endpoints.

## Planned Stack

- Frontend: React, TypeScript, Vite, TanStack Query, React Hook Form, Zod, Tailwind CSS
- Backend: ASP.NET Core Web API, EF Core, PostgreSQL, JWT auth

## Repository Layout

```text
myfi/
  docs/
    shared/
    coordination/
    frontend/
    backend/
    WORKLOG.md
  frontend/
    README.md
    docs/
    src/
  backend/
    README.md
    docs/
    src/
  .env.example
  docker-compose.yml
  AGENTS.md
  CONTRIBUTING.md
  README.md
```

## Docs To Read First

1. `docs/shared/README.md`
2. `docs/shared/product-scope.md`
3. `docs/shared/domain-model.md`
4. `docs/shared/auth-flow.md`
5. `docs/shared/api-contract.md`
6. `docs/shared/repo-structure.md`
7. `docs/coordination/README.md`
8. `docs/coordination/BOARD.md`
9. `docs/WORKLOG.md`
10. `CONTRIBUTING.md`
11. `frontend/README.md` or `backend/README.md` depending on the area you are changing

## Notes

- Keep the frontend and backend in this monorepo for now.
- Frontend scaffolding still needs to be initialized inside `frontend/`.
- Backend scaffolding now lives inside `backend/`.
- Update the shared docs first if the API contract or architecture changes.
- The backend is intentionally using a vertical-slice layout inside `MyFi.Api` instead of the earlier layered draft, with slice-local commands, validators, handlers, DTOs, and EF configuration.
- Use the local `frontend/README.md` and `backend/README.md` files as area-specific starting points for future work.
- Use `docs/coordination/` and `docs/WORKLOG.md` to manage planner-to-implementer handoff in markdown.
