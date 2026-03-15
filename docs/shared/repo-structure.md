# Concrete Folder Structure

This is the current repository shape plus the near-term target structure as implementation continues.

## Top-Level Layout

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

## Planned Frontend Structure

```text
frontend/
  README.md
  docs/
  src/
    app/
      providers/
      router/
    api/
    components/
      charts/
      forms/
      layout/
      feedback/
    features/
      auth/
      categories/
      dashboard/
      expenses/
      subscriptions/
    hooks/
    lib/
    pages/
    styles/
    types/
  public/
  index.html
  package.json
  tsconfig.json
  vite.config.ts
```

## Current Backend Structure

```text
backend/
  README.md
  docs/
    README.md
    local-development.md
  src/
    MyFi.Api/
      Common/
        Api/
        Behaviors/
        Persistence/
        Results/
        Security/
      Features/
        Users/
          Domain/
          Endpoints/
          GetCurrentUser/
          Login/
          Persistence/
          Shared/
          Signup/
      Program.cs
  tests/
    MyFi.Api.IntegrationTests/
      Support/
        Infrastructure/
        Seeding/
        Users/
  dotnet-tools.json
  MyFi.sln
```

## Backend Responsibility Split

- `MyFi.Api/Common`: cross-cutting API helpers, persistence, validation behavior, security, and shared result models
- `MyFi.Api/Features/*`: slice-local endpoints, handlers, validators, DTOs, entities, and EF configuration
- `tests/MyFi.Api.IntegrationTests`: end-to-end API tests plus shared test support and seeding helpers

## Frontend Responsibility Split

- `app/`: providers, router, app bootstrap
- `api/`: HTTP client and API modules
- `features/`: resource-based screens, hooks, local view components, forms
- `components/`: shared UI elements reused across features
- `pages/`: route-level page composition
- `lib/`: utilities, formatters, auth storage helpers
- `types/`: shared frontend-only TypeScript models where needed

## Ownership Guidance

- Frontend dev owns `frontend/` and uses `docs/shared/` plus `docs/frontend/`
- Backend dev owns `backend/` and uses `docs/shared/` plus `docs/backend/`
- Planner and coordinator work lives in `docs/coordination/` plus `docs/WORKLOG.md`
- `frontend/README.md` and `backend/README.md` should be treated as local entrypoints for agents working in those areas.
- Contract changes must be updated first in `docs/shared/api-contract.md`
- Frontend structure under `frontend/` remains planned until the frontend scaffold is initialized.
